using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5;
    [SerializeField] private float jumpForce = 5;

    private Vector2 moveInput;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Run();
        FlipSpriteOnRun();

        Jump();
    }

    private void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        Debug.Log(moveInput);
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.linearVelocity.y);
        rb.linearVelocity = playerVelocity;
    }

    private void Jump()
    {
    }

    private void FlipSpriteOnRun()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > Mathf.Epsilon)
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
