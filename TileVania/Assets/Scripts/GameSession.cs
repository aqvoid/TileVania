using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;

    private UIController uiController;

    private void Awake()
    {
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;

        if (numberGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        uiController = GetComponentInChildren<UIController>();
    }

    private void Start()
    {
        uiController.ChangeHealthText();
    }

    private void TakeLife()
    {
        playerLives--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        uiController.ChangeHealthText();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1) Invoke("TakeLife", 0.5f);
        else Invoke("ResetGameSession", 0.5f);
    }

    public int GetHealth() => playerLives;
}
