using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 5f;

    private PlayerMortality playerMortality;
    private PlayerMovement playerMovement;

    private PlayerInput playerInput;
    private InputAction attackAction;


    private void Awake()
    {
        playerMortality = GetComponentInParent<PlayerMortality>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        attackAction = playerInput.actions["Attack"];
        attackAction.performed += OnAttack;
    }

    private void OnDestroy() => attackAction.performed -= OnAttack;

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!playerMortality.IsAlive) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        if (direction.x != 0)
        {
            float facing = Mathf.Sign(direction.x);
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * facing, 1f);
        }

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

        bulletRb.linearVelocity = direction * bulletSpeed + playerMovement.GetVelocity();
        bulletRb.angularVelocity = Random.Range(-180, 180);
    }
}
