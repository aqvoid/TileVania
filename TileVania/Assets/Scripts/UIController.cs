using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private GameSession gameSession;

    private void Awake()
    {
        gameSession = GetComponentInParent<GameSession>();
    }

    public void ChangeHealthText()
    {
        healthText.text = $"Health: {gameSession.GetHealth()}";
    }

}
