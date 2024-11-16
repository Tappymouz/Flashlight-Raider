using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [Header("Timer Settings")]
    public float gameDuration = 300f; 
    private float remainingTime;

    [Header("Gameplay Control")]
    public bool isGamePaused = false;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverEnemyCountText;
    public TextMeshProUGUI gameplayEnemyCountText;
    public TextMeshProUGUI currencyEarnedTextVictory;
    public TextMeshProUGUI currencyEarnedTextGameover;

    [Header("Currency")]
    public int temporaryCurrencyHolder = 0;

    [Header("Player Character Setup")]
    public GameObject[] playerCharacters;
    public GameObject reloadButton;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this; // Set the instance to this instance
        DontDestroyOnLoad(gameObject); // Optionally keep GameManager across scenes
    }
    private void Start()
    {
        ActivateEquippedCharacter();
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        
        AudioManager.Instance.PlayMusic("Gameplay");

        // Start the timer
        remainingTime = gameDuration;
        StartCoroutine(TimerCountdown());
        UpdateEnemyCount();
    }

    private void Update()
    {
        if (remainingTime <= 0 && !gameOverPanel.activeSelf)
        {
            GameOver();
        }


        UpdateTimerUI();
        UpdateEnemyCount();

        if (GameObject.FindObjectsOfType<EnemyMovement>().Length == 0 && !victoryPanel.activeSelf)
        {
            GameVictory();
        }
    }
    private IEnumerator TimerCountdown()
    {
        while (remainingTime > 0)
        {
            if (!isGamePaused)
            {
                remainingTime -= Time.deltaTime;
            }
            yield return null;
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateEnemyCount()
    {
        int enemyCount = GameObject.FindObjectsOfType<EnemyMovement>().Length;

        if (gameplayEnemyCountText != null)
        {
            gameplayEnemyCountText.text = "" + enemyCount;
        }

        if (gameOverEnemyCountText != null && gameOverPanel.activeSelf)
        {
            gameOverEnemyCountText.text = "" + enemyCount;
        }
    }

    private void ActivateEquippedCharacter()
    {
        int equippedId = PlayerDataManager.instance.playerData.equippedCharacterId;

        // Make sure all characters are disabled first
        foreach (var character in playerCharacters)
        {
            character.SetActive(false);
        }

        // Activate the character with the equippedCharacterId
        if (equippedId >= 0 && equippedId < playerCharacters.Length)
        {
            playerCharacters[equippedId].SetActive(true);
        }
        else
        {
            Debug.LogError("Equipped character ID is out of range. Check your PlayerData setup.");
        }
        UpdateReloadButtonState(equippedId);
    }

    public void UpdateReloadButtonState(int equippedId)
    {
        if (reloadButton != null)
        {
            // Enable the reload button only if the equipped character is the gun
            reloadButton.SetActive(equippedId == 3);
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0; 

        AudioManager.Instance.PlaySFX("Click"); 
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1; 

        AudioManager.Instance.PlaySFX("Click");
    }

    private void GameOver()
    {
        isGamePaused = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        if (currencyEarnedTextGameover != null)
        {
            currencyEarnedTextGameover.text = "" + temporaryCurrencyHolder;
        }

        UpdateEnemyCount();
        SaveCurrency();
        AudioManager.Instance.PlayMusic("Gameover"); 
    }

    private void GameVictory()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        victoryPanel.SetActive(true);
        AddCurrency(50);
        if (currencyEarnedTextVictory != null)
        {
            currencyEarnedTextVictory.text = "" + temporaryCurrencyHolder;
        }
        SaveCurrency();
        AudioManager.Instance.PlayMusic("GameVictory");
    }

    public void OpenPauseMenu()
    {
        if (!isGamePaused)
        {
            PauseGame();
        }
    }

    public void ClosePauseMenu()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
    }

    public void ReturnToMainMenu()
    {
        if (reloadButton != null)
        {
            reloadButton.SetActive(false);
        }

        Time.timeScale = 1;
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu"); 
    }

    public void AddCurrency(int amount)
    {
        temporaryCurrencyHolder += amount;
        Debug.Log("Currency added: " + amount + ", Total: " + temporaryCurrencyHolder);
    }

    private void SaveCurrency()
    {
        PlayerDataManager.instance.playerData.currency += temporaryCurrencyHolder;
        PlayerDataManager.instance.SavePlayerData();
        temporaryCurrencyHolder = 0;
    }

    public void DestroyAllEnemies()
    {
        // Find all objects with the EnemyMovement component
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        // Destroy each enemy object found
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }

        Debug.Log("All enemies destroyed!");
    }
}
