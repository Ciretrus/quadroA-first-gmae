using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ItemsActivations), typeof(ThiefEye))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private Transform m_cameraPos;
    [SerializeField] private Transform m_groundCheckerPivot;
    [SerializeField] private float m_gravity = 9.8f;
    [SerializeField] private float m_groundDrag;
    [SerializeField] private float m_airMultiplier;

    [Header("Speed settings")]
    [SerializeField] private float m_smoothInputSpeed = 0.2f;
    [SerializeField] private float m_walkSpeed = 10f;
    [SerializeField] private float m_sprintSpeed = 20f;
    [SerializeField] private float m_sneakSpeed = 5f;
    [SerializeField] private float m_jumpForce = 5f;

    [Header("Ground settings")]
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField] private float m_checkGroundRadius = 0.3f;

    [Header("Slope settings")]
    [SerializeField] private float m_maxSlopeAngle = 50f;
    [SerializeField] private float m_minSlopeAngle = 3f;

    private ItemsActivations m_itemsActivations;
    private Diary m_diary;
    private ThiefEye m_thiefEye;
    private UIController m_uiController;
    private PlayerInput m_input;
    private RaycastHit m_slopeHit;
    private Vector2 m_smoothVector;
    private Vector2 m_smoothVelocity;
    private float m_currentSpeed;
    private bool m_isSprinting;
    private bool m_isSneaking;

    public Transform cameraPos => m_cameraPos;
    public PlayerInput input => m_input;
    public bool isMoving { get; private set; } = false;
    public bool isGrounded => IsGrounded();
    public bool isSprinting => m_isSprinting;
    public bool isSneaking => m_isSneaking;

    private void Awake()
    {
        m_input = new PlayerInput();
        m_input.Enable();
    }

    private void OnEnable()
    {
        m_itemsActivations = ServiceLocator.Resolve<ItemsActivations>();
        m_diary = ServiceLocator.Resolve<Diary>();
        m_thiefEye = ServiceLocator.Resolve<ThiefEye>();
        m_uiController = ServiceLocator.Resolve<UIController>();

        m_input.Movement.Jump.performed += Jump;
        m_input.Movement.Sprint.performed += Sprint;
        m_input.Movement.Sprint.canceled += StopSprint;
        m_input.Movement.Sneak.performed += Sneak;
        m_input.Movement.Sneak.canceled += StopSneak;

        m_input.UI.Diary.performed += m_itemsActivations.ChangeUIMode;
        m_input.UI.Diary.performed += m_diary.ChangeState;
        m_input.UI.ThiefEye.performed += m_thiefEye.ActivateThiefEye;
        m_input.UI.Pause.performed += m_uiController.ShowPauseMenu;
        m_input.UI.Inventory.performed += m_uiController.ShowInventory;
    }

    private void OnDisable()
    {
        m_input.Movement.Jump.performed -= Jump;
        m_input.Movement.Sprint.performed -= Sprint;
        m_input.Movement.Sprint.canceled -= StopSprint;
        m_input.Movement.Sneak.performed -= Sneak;
        m_input.Movement.Sneak.canceled -= StopSneak;

        m_input.UI.Diary.performed -= m_itemsActivations.ChangeUIMode;
        m_input.UI.Diary.performed -= m_diary.ChangeState;
        m_input.UI.ThiefEye.performed -= m_thiefEye.ActivateThiefEye;
        m_input.UI.Pause.performed -= m_uiController.ShowPauseMenu;
    }

    private void OnDestroy()
    {
        m_input.Disable();
    }

    private void FixedUpdate()
    {   
        Move();
        ControlSpeed();
        if (IsGrounded() || IsOnSlope())
        {
            m_rigidbody.linearDamping = m_groundDrag;
            if (m_input.Movement.Walk.ReadValue<Vector2>() == Vector2.zero && IsOnSlope())
            { 
                m_rigidbody.linearDamping = m_groundDrag*10;
            }
            
        }
        else
        {
            m_rigidbody.linearDamping = 0;
        }

    }

    public void SetPosition(Transform newTransform)
    {
        transform.position = newTransform.position;

        float newRotY = newTransform.localRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(transform.rotation.x, newRotY, transform.rotation.z);
    }

    public void ChangeMovementState()
    {
        if (m_input.Movement.enabled)
        {
            m_input.Movement.Disable();
        }
        else
        {
            m_input.Movement.Enable();
        }
    }

    private void Move()
    {
        //m_rigidbody.useGravity = !IsOnSlope();
        Vector2 inputVector = m_input.Movement.Walk.ReadValue<Vector2>();

        if (inputVector == Vector2.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        m_smoothVector = Vector2.SmoothDamp(m_smoothVector, inputVector, ref m_smoothVelocity, m_smoothInputSpeed);
        Vector3 moveDirection = new Vector3(m_smoothVector.x, 0f, m_smoothVector.y);

        if (moveDirection.magnitude >= 1f)
        {
            moveDirection.Normalize();
        }

        if (m_isSprinting)
        {
            m_currentSpeed = m_sprintSpeed;
        }
        else if (m_isSneaking)
        {
            m_currentSpeed = m_sneakSpeed;
        }
        else
        {
            m_currentSpeed = m_walkSpeed;
        }

        if (IsGrounded())
        {
            moveDirection = SlopeMoveDirection(moveDirection);
            m_rigidbody.AddForce(transform.TransformDirection(moveDirection) * (m_currentSpeed * 10f), ForceMode.Force);
        }

        if (!IsGrounded())
        {
            m_rigidbody.AddForce(transform.TransformDirection(moveDirection) * (m_currentSpeed * 10f * m_airMultiplier), ForceMode.Force);
            m_rigidbody.AddForce(- Vector3.up * m_gravity, ForceMode.Force);
        }
    }

    private void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0f, m_rigidbody.linearVelocity.z);

        if (flatVelocity.magnitude > m_currentSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * m_currentSpeed;
            m_rigidbody.linearVelocity = new Vector3(limitedVelocity.x, m_rigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (m_isSneaking)
        {
            StopSneak(context);
        }

        if (IsGrounded())
        {
            m_rigidbody.linearVelocity = new Vector3(m_rigidbody.linearVelocity.x, 0f, m_rigidbody.linearVelocity.z);

            m_rigidbody.AddForce(transform.up * m_jumpForce * 2f, ForceMode.Impulse);
        }
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        if (IsGrounded() && !m_isSneaking)
        {
            m_isSprinting = true;
        }
    }

    private void StopSprint(InputAction.CallbackContext context)
    {
        m_isSprinting = false;
    }

    private void Sneak(InputAction.CallbackContext context)
    {
        if (IsGrounded() && !m_isSprinting)
        {
            m_isSneaking = true;

            m_cameraPos.transform.position -= new Vector3(0f, 0.5f, 0f);
        }
    }

    private void StopSneak(InputAction.CallbackContext context)
    {
        if (m_isSneaking)
        {
            m_isSneaking = false;

            m_cameraPos.transform.position += new Vector3(0f, 0.5f, 0f);
        }
    }

    private bool IsGrounded()
    {
        bool groundCheck = Physics.CheckSphere(m_groundCheckerPivot.position, m_checkGroundRadius, m_groundMask);
        return groundCheck;
    }

    private bool IsOnSlope()
    {
        bool slopeCheck = Physics.Raycast(m_groundCheckerPivot.position, Vector3.down, out m_slopeHit, m_checkGroundRadius);
        if (slopeCheck)
        {
            float angle = Vector3.Angle(Vector3.up, m_slopeHit.normal);
            if (angle > m_minSlopeAngle && angle < m_maxSlopeAngle)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 SlopeMoveDirection(Vector3 moveDir)
    {
        Vector3 slopeDir = Vector3.ProjectOnPlane(moveDir, m_slopeHit.normal);
        return slopeDir;
    }
}