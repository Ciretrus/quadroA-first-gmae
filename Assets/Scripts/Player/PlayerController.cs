using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private GameObject m_head;
    [SerializeField] private float m_walkSpeed = 10f;
    [SerializeField] private float m_sprintSpeed = 20f;
    [SerializeField] private float m_sneakSpeed = 5f;
    [SerializeField] private float m_jumpForce = 5f; 
    [SerializeField] private Transform _groundCheckerPivot;
    [SerializeField] private float _checkGroundRadius = 0.3f;
    [SerializeField] private LayerMask _groundMask;
    private PlayerInput m_input;
    private Vector3 m_velocity;
    private float m_currentSpeed;
    private bool m_isSprinting;
    private bool m_isSneaking;
    private float m_gravity = 9.8f;
    public bool isMoving { get; private set; } = false;
    public bool isGrounded => m_characterController.isGrounded;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        m_input = new PlayerInput();
        m_input.Player.Enable();

        m_input.Player.Jump.performed += Jump;
        m_input.Player.Sprint.performed += Sprint;
        m_input.Player.Sprint.canceled += StopSprint;
        m_input.Player.Sneak.performed += Sneak;
        m_input.Player.Sneak.canceled += StopSneak;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (IsGrounded() && m_velocity.y < 0)
        {
            m_velocity.y = 0f;
        }

        Vector2 inputVector = m_input.Player.Walk.ReadValue<Vector2>();

        if (inputVector == Vector2.zero)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

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