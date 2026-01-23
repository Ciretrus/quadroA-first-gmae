using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    [SerializeField] private CharacterController m_characterController;
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
    public bool isGrounded => m_characterController.isGrounded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);

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
    }

    public void SetPosition(Vector3 position)
    {
        m_characterController.Move(position);
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
}