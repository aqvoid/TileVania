using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    private Vector2 moveInput;

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
    }
    private bool IsMoving() => Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        Debug.Log(moveInput);
    }

    private void OnJump(InputValue inputValue)
    {
        if (!col.IsTouchingLayers(LayerMask.GetMask("Surface"))) return;
        if (inputValue.isPressed) rb.linearVelocity += new Vector2(0f, jumpForce);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;

        FlipSpriteOnRun();
        AnimateOnRun();
    }

    private void AnimateOnRun()
    {
        if (IsMoving()) anim.SetBool("isRunning", true);
        else anim.SetBool("isRunning", false);
    }
    private void FlipSpriteOnRun()
    {
        if (IsMoving())
            sr.flipX = rb.linearVelocity.x < 0;
    }
}
