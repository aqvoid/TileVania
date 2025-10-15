using UnityEngine;

public class PlayerMortality : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true;

    public void PlayerDeath(CapsuleCollider2D playerBodyCollider, Animator playerAnimator, Rigidbody2D playerRigidbody2D)
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        { 
            IsAlive = false;
            playerAnimator.SetTrigger("isDying");
            playerRigidbody2D.linearVelocity += new Vector2(0f, 10f);
        }
    }
}
