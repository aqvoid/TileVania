using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 5f;

    private PlayerMortality playerMortality;

    private PlayerInput playerInput;
    private InputAction attackAction;


    private void Awake()
    {
        playerMortality = GetComponentInParent<PlayerMortality>();
        playerInput = GetComponentInParent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
        attackAction.performed += OnAttack;
    }

    private void OnDestroy() => attackAction.performed -= OnAttack;

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!playerMortality.IsAlive) return;
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);

        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        bulletRb.linearVelocity = new Vector2(transform.parent.localScale.x * bulletSpeed, bulletRb.linearVelocity.y);
    }
}
