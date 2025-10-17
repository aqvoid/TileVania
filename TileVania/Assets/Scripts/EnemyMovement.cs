using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private CapsuleCollider2D bodyCol;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        bodyCol = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void RotateAndFlip()
    {
        if (sr.flipX) sr.flipX = false;
        else sr.flipX = true;
        boxCol.offset = new Vector2(-boxCol.offset.x, boxCol.offset.y);
        moveSpeed = -moveSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        RotateAndFlip();
    }
}
