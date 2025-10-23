using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float climbSpeed;
    [SerializeField, Range(1, 4)] private float startGravity = 2f;

    [Header("=== Dash ===")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;

    private Vector2 moveInput;
    private bool canDash = true;
    private bool isDashing = false;

    private Rigidbody2D rb;
    private Animator anim;
    private CapsuleCollider2D bodyCol;
    private BoxCollider2D feetCol;
    private PlayerMortality playerMortality;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private bool IsClimbing() => Mathf.Abs(moveInput.y) > Mathf.Epsilon;

    private void OnMove(InputValue inputValue)
    {
        if (!playerMortality.IsAlive) return;
        moveInput = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (!playerMortality.IsAlive) return;
        if (inputValue.isPressed && feetCol.IsTouchingLayers(LayerMask.GetMask("Surface")))
            rb.linearVelocity += new Vector2(0f, jumpForce);
    }

    private void OnSprint(InputValue inputValue)
    {
        if (!playerMortality.IsAlive) return;
        if (inputValue.isPressed && canDash && !isDashing) StartCoroutine(DashCooldown());
    }

    private void Run()
    {
        if (!isDashing)
            rb.linearVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);

        FlipSpriteOnRun();
        AnimateOnRun();
    }

    private void Climb()
    {
        if (bodyCol.IsTouchingLayers(LayerMask.GetMask("Ladder")) && IsClimbing())
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveInput.y * climbSpeed);
            IsAnimateOnClimb(true);
        }
        else
        {
            rb.gravityScale = startGravity;
            IsAnimateOnClimb(false);
        }
    }

    private void AnimateOnRun()
    {
        if (IsMoving()) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);
    }
    
    private void IsAnimateOnClimb(bool state)
    {
        anim.SetBool("isClimbing", state);
    }

    private void FlipSpriteOnRun()
    {
        if (IsMoving()) transform.localScale = new Vector2(Mathf.Sign(rb.linearVelocity.x), 1f);
    }

    private IEnumerator DashCooldown()
    {
        // Dashing

        canDash = false;
        isDashing = true;

        rb.gravityScale = 0.25f;
        rb.linearVelocity = new Vector2 (moveInput.x * dashSpeed, rb.linearVelocity.y);
        
        yield return new WaitForSeconds(dashDuration); // Immediately After Dash

        isDashing = false;

        rb.gravityScale = startGravity;

        yield return new WaitForSeconds(dashCooldown); // After Dash Cooldown

        canDash = true;
    }

    public Vector2 GetVelocity() => rb.linearVelocity;
}
