using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField, Range(1, 4)] private float startGravity = 2f;

    private Vector2 moveInput;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private CapsuleCollider2D bodyCol;
    private BoxCollider2D feetCol;
    
    private PlayerMortality playerMortality;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        bodyCol = GetComponent<CapsuleCollider2D>();
        feetCol = GetComponent<BoxCollider2D>();
        playerMortality = GetComponent<PlayerMortality>();
    }

    private void FixedUpdate()
    {
        if (!playerMortality.IsAlive) return;
        Run();
        Climb();
        playerMortality.PlayerDeath(bodyCol, anim, rb);
    }

    private bool IsMoving() => Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;
    private bool IsClimbing() => Mathf.Abs(rb.linearVelocity.y) > Mathf.Epsilon;

    private void OnMove(InputValue inputValue)
    {
        if (!playerMortality.IsAlive) return;
        moveInput = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (!playerMortality.IsAlive) return;
        if (!feetCol.IsTouchingLayers(LayerMask.GetMask("Surface"))) return;
        if (inputValue.isPressed) rb.linearVelocity += new Vector2(0f, jumpForce);
    }

    private void Run()
    {
        rb.linearVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);

        FlipSpriteOnRun();
        AnimateOnRun();
    }

    private void Climb()
    {
        if (!feetCol.IsTouchingLayers(LayerMask.GetMask("Ladder")))
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
