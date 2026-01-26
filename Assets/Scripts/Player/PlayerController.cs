using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [SerializeField] private CharacterController m_characterController;
    // [SerializeField] private Rigidbody m_rigidbody;
    // [SerializeField] private float m_groundDrag;
    // [SerializeField] private float m_airMultiplier;
    // [SerializeField] private Transform m_orientation;
    [SerializeField] private ItemsActivations m_itemsActivations;
    [SerializeField] private ThiefEye m_thiefEye;
    [SerializeField] private GameObject m_head;
    [SerializeField] private Transform _groundCheckerPivot;
    [SerializeField] private float m_smoothInputSpeed = 0.2f;
    [SerializeField] private float m_walkSpeed = 10f;
    [SerializeField] private float m_sprintSpeed = 20f;
    [SerializeField] private float m_sneakSpeed = 5f;
    [SerializeField] private float m_jumpForce = 5f;
    [SerializeField] private float m_gravity = 9.8f;
    [SerializeField] private float _checkGroundRadius = 0.3f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Diary m_diary;

    private Vector3 m_velocity;
    private Vector2 m_smoothVector;
    private Vector2 m_smoothVelocity;
    private float m_currentSpeed;
    private bool m_isSprinting;
    private bool m_isSneaking;
    private PlayerInput m_input;

    public PlayerInput input => m_input;
    public bool isMoving { get; private set; } = false;
    public bool isGrounded => IsGrounded();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

        // m_rigidbody.freezeRotation = true;

        m_input = new PlayerInput();
        m_input.Enable();

        m_input.Movement.Jump.performed += Jump;
        m_input.Movement.Sprint.performed += Sprint;
        m_input.Movement.Sprint.canceled += StopSprint;
        m_input.Movement.Sneak.performed += Sneak;
        m_input.Movement.Sneak.canceled += StopSneak;

        m_input.UI.Diary.performed += m_diary.ChangeState;
        m_input.UI.Diary.performed += m_itemsActivations.ChangeUIMode;
        m_input.UI.ThiefEye.performed += m_thiefEye.ActivateThiefEye;
    }

    private void OnDestroy()
    {
        m_input.Disable();
    }

    private void Update()
    {
        Move();

        /*if (isGrounded)
        {
            m_rigidbody.drag = m_groundDrag;
        }
        else
        {
            m_rigidbody.drag = 0;
        }*/
    }

    public void SetPosition(Vector3 position)
    {
        m_characterController.enabled = false;

        float playerY = m_characterController.transform.position.y;
        m_characterController.transform.position = new Vector3(position.x, playerY, position.z);

        m_characterController.enabled = true;
    }

    private void Move()
    {
        if (IsGrounded() && m_velocity.y < 0)
        {
            m_velocity.y = 0f;
        }

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

        // TODO Remove "Move" methods
        // Don't use CharacterController
        // Read all comments in this script!
        // transform.position += transform.TransformDirection(moveDirection) * m_currentSpeed * Time.deltaTime;

        m_characterController.Move(transform.TransformDirection(moveDirection) * (m_currentSpeed * Time.deltaTime));

        m_velocity.y -= m_gravity * Time.deltaTime; 
        m_characterController.Move(m_velocity * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (m_isSneaking)
        {
            StopSneak(context);
        }

        if (IsGrounded())
        {
            m_velocity.y += m_jumpForce;
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

            m_head.transform.position -= new Vector3(0f, 0.5f, 0f);
        }
    }

    private void StopSneak(InputAction.CallbackContext context)
    {
        if (m_isSneaking)
        {
            m_isSneaking = false;

            m_head.transform.position += new Vector3(0f, 0.5f, 0f);
        }
    }

    private bool IsGrounded()
    {
        bool groundCheck = Physics.CheckSphere(_groundCheckerPivot.position, _checkGroundRadius, _groundMask);
        return groundCheck;
    }

    /*private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = m_orientation.forward * verticalInput + m_orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rm_rigidbodyb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            m_rigidbody.AddForce(moveDirection.normalized * moveSpeed * 10f * m_airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(m_rigidbody.velocity.x, 0f, m_rigidbody.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            m_rigidbody.velocity = new Vector3(limitedVel.x, m_rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0f, m_rigidbody.velocity.z);

        m_rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }*/
}