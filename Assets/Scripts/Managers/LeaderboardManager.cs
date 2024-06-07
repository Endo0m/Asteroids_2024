using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI leaderboardText;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject leaderbordPanel;

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    private void Start()
    {
        LoadLeaderboard();
        submitButton.onClick.AddListener(SubmitScore);
        nameInputField.onEndEdit.AddListener(OnNameInputEndEdit);
        nameInputField.onSubmit.AddListener(OnNameInputSubmit);
        nameInputField.onValidateInput += ValidateInput;
    }
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SelectPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SelectNext();
        }
    }

    private void SelectPrevious()
    {
        Selectable current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        if (current != null)
        {
            Selectable previous = current.FindSelectableOnUp();
            if (previous != null)
            {
                previous.Select();
            }
        }
    }

    private void SelectNext()
    {
        Selectable current = EventSystem.current.currentSelectedGameObject?.GetComponent<Selectable>();
        if (current != null)
        {
            Selectable next = current.FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }

    private void OnEnable()
    {
        DisplayLeaderboard();
    }

    private void OnNameInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (ValidateName(text))
            {
                submitButton.onClick.Invoke();
            }
        }
    }

    private void OnNameInputSubmit(string text)
    {
        if (ValidateName(text))
        {
            submitButton.onClick.Invoke();
        }
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        if (char.IsWhiteSpace(addedChar) || char.IsDigit(addedChar) || !char.IsLetter(addedChar))
        {
            return '\0'; 
        }
        return addedChar;
    }

    private bool ValidateName(string text)
    {
        if (text.Length > 8)
        {
            text = text.Substring(0, 8);
            nameInputField.text = text;
        }

        if (ContainsInvalidCharacters(text))
        {
            ShowWarning("Цифры и знаки низя=)");
            return false;
        }

        if (text.Length < 3)
        {
            ShowWarning("Введите не менее 3 символов");
            return false;
        }

        return true;
    }

    private static bool ContainsInvalidCharacters(string text)
    {
        string pattern = @"[\s\d\W]";
        return Regex.IsMatch(text, pattern);
    }

    private void SubmitScore()
    {
        string playerName = nameInputField.text;

        if (!ValidateName(playerName))
        {
            return;
        }

        int score = GameManager.Instance.GetScore();

        LeaderboardEntry existingEntry = leaderboard.Find(entry => entry.playerName == playerName);

        if (existingEntry != null)
        {
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
            }
        }
        else
        {
            LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score);
            leaderboard.Add(newEntry);
        }

        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.Count > 10)
        {
            leaderboard.RemoveRange(10, leaderboard.Count - 10);
        }

        SaveLeaderboard();
        DisplayLeaderboard();
        nameInputField.text = "";
        warningText.text = "";
        gameOverPanel.SetActive(false);
        leaderbordPanel.SetActive(true);
        SoundManager.Instance.PlayLeaderBoardSound();
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
    }

    private void DisplayLeaderboard()
    {
        StringBuilder leaderboardString = new StringBuilder();
        leaderboardString.AppendLine($"{"Место",-10} {"Имя",-10} {"Счет",-10}");

        for (int i = 0; i < leaderboard.Count; i++)
        {
            LeaderboardEntry entry = leaderboard[i];
            leaderboardString.AppendLine($"{i + 1,-10} {entry.playerName,-10} {entry.score,-10}");
        }

        leaderboardText.text = leaderboardString.ToString();
    }

    private void SaveLeaderboard()
    {
        PlayerPrefs.SetInt("LeaderboardCount", leaderboard.Count);

        for (int i = 0; i < leaderboard.Count; i++)
        {
            LeaderboardEntry entry = leaderboard[i];
            PlayerPrefs.SetString($"LeaderboardName{i}", entry.playerName);
            PlayerPrefs.SetInt($"LeaderboardScore{i}", entry.score);
        }

        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        int count = PlayerPrefs.GetInt("LeaderboardCount", 0);

        for (int i = 0; i < count; i++)
        {
            string playerName = PlayerPrefs.GetString($"LeaderboardName{i}");
            int score = PlayerPrefs.GetInt($"LeaderboardScore{i}");

            LeaderboardEntry entry = new LeaderboardEntry(playerName, score);
            leaderboard.Add(entry);
        }

        AddInitialEntries();
    }

    private void AddInitialEntries()
    {
        List<LeaderboardEntry> initialEntries = new List<LeaderboardEntry>
        {
            new LeaderboardEntry("Путин", 656649),
            new LeaderboardEntry("Трамп", 25000),
            new LeaderboardEntry("Макрон", 18000),
            new LeaderboardEntry("Меркель", 15000),
            new LeaderboardEntry("Байден", 6666),
            new LeaderboardEntry("Frank", 6500),
            new LeaderboardEntry("Grace", 6000),
            new LeaderboardEntry("Hank", 5500),
            new LeaderboardEntry("Ivy", 5000),
            new LeaderboardEntry("Jack", 4500)
        };

        foreach (var entry in initialEntries)
        {
            if (!leaderboard.Exists(e => e.playerName == entry.playerName))
            {
                leaderboard.Add(entry);
            }
        }

        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        if (leaderboard.Count > 10)
        {
            leaderboard.RemoveRange(10, leaderboard.Count - 10);
        }
    }

    private class LeaderboardEntry
    {
        public string playerName;
        public int score;

        public LeaderboardEntry(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }
}