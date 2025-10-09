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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Run();
        Jump();
    }
    private bool IsMoving() => Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon;

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        Debug.Log(moveInput);
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

    private void Jump()
    {
    }

    private void FlipSpriteOnRun()
    {
        if (IsMoving())
            sr.flipX = rb.linearVelocity.x < 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            //Run();
            //Jump();
        }
    }
}
