using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance { get; private set; }

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button exitButton;

    private bool isPaused = false;
    private int currentButtonIndex = 0;
    private Button[] buttons;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        closeButton.onClick.AddListener(Resume);
        exitButton.onClick.AddListener(() => StartCoroutine(QuitGame()));

        buttons = new Button[] { closeButton, exitButton };
        UpdateButtonSelection();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                currentButtonIndex = (currentButtonIndex - 1 + buttons.Length) % buttons.Length;
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
                UpdateButtonSelection();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                buttons[currentButtonIndex].onClick.Invoke();
            }
        }
    }

    private void UpdateButtonSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    public void Resume()
    {
        SoundManager.Instance.PlayCloseWindowSound();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsLeaderboardActive())
        {
            return;
        }

        SoundManager.Instance.PlayOpenWindowSound();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        UpdateButtonSelection();
    }

    private IEnumerator QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = true;
        SoundManager.Instance.PlayExitGameSound();
        yield return new WaitForSeconds(1f); 
        Application.Quit();
    }
}