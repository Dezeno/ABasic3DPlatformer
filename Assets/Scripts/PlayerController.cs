using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    private Vector2 m_moveInput;

    [SerializeField] private LayerMask m_groundLayer;
    [SerializeField] private float m_groundCheckDistance = 0.1f;

    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_jumpForce = 5f;
    private bool m_isJumping = false;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        if (m_rigidbody == null)
        {
            Debug.LogError("Rigidbody missing on " + gameObject.name);
        }  
    }

    private void FixedUpdate()
    {
        float currentY = m_rigidbody.linearVelocity.y;
        
        m_rigidbody.linearVelocity = m_moveInput.magnitude > 0.01f 
            ? new Vector3(m_moveInput.x * m_moveSpeed, currentY, m_moveInput.y * m_moveSpeed) 
            : new Vector3(0f, currentY, 0f);

        if (m_isJumping)
        {
            m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            m_isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down,
            GetComponent<Collider>().bounds.extents.y + m_groundCheckDistance,
            m_groundLayer);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            m_isJumping = true;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_moveInput = context.ReadValue<Vector2>();
    }
}