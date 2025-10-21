using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float bulletCooldown = 1f;

    private PlayerMortality playerMortality;
    private PlayerMovement playerMovement;

    private Vector2 mousePos;
    private Vector2 direction;
    private bool isShooting = false;
    private bool canShoot = true;

    private PlayerInput playerInput;
    private InputAction attackAction;

    private void Awake()
    {
        playerMortality = GetComponentInParent<PlayerMortality>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerMovement = GetComponentInParent<PlayerMovement>();

        attackAction = playerInput.actions["Attack"];
    }

    private void OnEnable()
    {
        attackAction.started += OnAttackStarted;
        attackAction.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        attackAction.started -= OnAttackStarted;
        attackAction.canceled -= OnAttackCanceled;
    }

    private void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (!playerMortality.IsAlive) return;
        if (isShooting) return;
        isShooting = true;
        StartCoroutine(AutoShoot());
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    private IEnumerator AutoShoot()
    {
        while (isShooting)
        {
            if (canShoot)
            {
                canShoot = false;

                SaveShootPosition();
                ChangeFacingDirection();
                CreateBullet();

                yield return new WaitForSeconds(bulletCooldown);
                canShoot = true;
            }
            yield return null;
        }
    }

    private void CreateBullet()
    {
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

        bulletRb.linearVelocity = direction * bulletSpeed + playerMovement.GetVelocity();
        bulletRb.angularVelocity = Random.Range(-180, 180);
    }

    private void ChangeFacingDirection()
    {
        if (direction.x != 0)
        {
            float facing = Mathf.Sign(direction.x);
            transform.parent.localScale = new Vector2(Mathf.Abs(transform.parent.localScale.x) * facing, 1f);
        }
    }

    private void SaveShootPosition()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (mousePos - (Vector2)transform.position).normalized;
    }
}
