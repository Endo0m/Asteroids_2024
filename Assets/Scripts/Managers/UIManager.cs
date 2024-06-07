using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject scoreOverUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject leaderbordPanel;
    [SerializeField] private Text livesText;
    [SerializeField] private Text scoreText;
    [SerializeField] private InputField nameInputField; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DiePlayer()
    {
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        playerHealth.OnDestroy();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = $"{lives}";
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
    }

    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
        SetFocusOnInputField();
    }

    public void HideGameOverUI()
    {
        gameOverUI.SetActive(false);
    }

    public void ShowScoreOverUI()
    {
        scoreOverUI.SetActive(true);
    }

    public void HideScoreOverUI()
    {
        scoreOverUI.SetActive(false);
    }

    public bool IsLeaderboardActive()
    {
        return leaderbordPanel.activeSelf;
    }

    public void ShowPauseMenuUI()
    {
        pauseMenuUI.SetActive(true);
    }

    public void HidePauseMenuUI()
    {
        pauseMenuUI.SetActive(false);
    }

    private void SetFocusOnInputField()
    {
        EventSystem.current.SetSelectedGameObject(nameInputField.gameObject, null);
    }
}
