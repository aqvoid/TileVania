using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;

    private Vector2 moveInput;
    private float startGravity = 1.5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        Run();
        Climb();
    }

    private bool IsMoving() => Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
    private bool IsClimbing() => Mathf.Abs(rb.linearVelocity.y) > Mathf.Epsilon;

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (!col.IsTouchingLayers(LayerMask.GetMask("Surface"))) return;
        if (inputValue.isPressed) rb.linearVelocity += new Vector2(0f, jumpForce);
    }

    private void Run()
    {
        if (!col.IsTouchingLayers(LayerMask.GetMask("Surface")) &&
            !col.IsTouchingLayers(LayerMask.GetMask("Ladder"))) return;

        rb.linearVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);

        FlipSpriteOnRun();
        AnimateOnRun();
    }

    private void Climb()
    {
        if (!col.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            rb.gravityScale = startGravity;
            return;
        }

        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * climbSpeed);

        AnimateOnClimb();
    }

    private void AnimateOnRun()
    {
        if (IsMoving()) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);
    }
    
    private void AnimateOnClimb()
    {
        if (IsClimbing()) anim.SetBool("isClimbing", true);
        else anim.SetBool("isClimbing", false);
    }

    private void FlipSpriteOnRun()
    {
        if (IsMoving())
            sr.flipX = rb.linearVelocity.x < 0;
    }
}
