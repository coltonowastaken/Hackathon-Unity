using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float JumpForce = 10f;
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.2f;
    public LayerMask GroundLayer;


    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float MoveInput = 0f;

        if (Gamepad.current != null)
        {
            MoveInput = Gamepad.current.leftStick.x.ReadValue();
        }
        else if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) MoveInput = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) MoveInput = 1f;
        }

        rb.linearVelocity = new Vector2(MoveInput * MoveSpeed, rb.linearVelocity.y);
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }
        setAnimation(MoveInput);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);
    }

    private void setAnimation(float MoveInput)
    {
        if (isGrounded)
        {
            if (MoveInput == 0)
            {
                animator.Play("Player_Idle", 0);
            }
            else
            {
                animator.Play("Player_Run", 0);
            }
        }
        else
        {
            if (rb.linearVelocity.y > 0)
            {
                animator.Play("Player_Jump", 0);
            }
            else
            {
                animator.Play("Player_Fall", 0);
            }
        }


    }
}
