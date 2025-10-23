using UnityEngine;

public class PlayerMortality : MonoBehaviour
{
    private bool isAlive = true;

    private void FixedUpdate() => PlayerDeath();

    public void PlayerDeath()
    {
        if (GetComponent<CapsuleCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) && isAlive)
        { 
            isAlive = false;
            GetComponent<Animator>().SetTrigger("isDying");
            GetComponent<Rigidbody2D>().linearVelocity += new Vector2(0f, 10f);
            GetComponent<PlayerMovement>().enabled = false;
            GetComponentInChildren<PlayerGun>().enabled = false;
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }
}
