using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // ABSTRACTION

    public Text timerText;
    public Text gameOverText;
    public Button restartButton;
    public Button startButton;
    public GameObject titleText;
    public float rotationSpeed = 50f;

    private float timer = 0f;
    private float countdownDuration = 10f;
    private bool isCountdownRunning = false;
    private bool isGameOver = false;
    private bool gameStarted = false;

    public Text counterText;
    public Text bestScoreName;

    private Counter counterScript;

  

    public void Start()
    {
       
        isCountdownRunning = false;

        MainManager.Instance.LoadData();
        bestScoreName.text = $"Best Score: {MainManager.Instance.bestScoreText} : {MainManager.Instance.bestScore}";


        if (counterText == null)
        {
            Debug.LogError("El campo counterText no está asignado en el UIManager.");
        }

        if (timerText == null)
        {
            Debug.LogError("El campo timerText no está asignado en el UIManager.");
        }

        if (gameOverText == null)
        {
            Debug.LogError("El campo gameOverText no está asignado en el UIManager.");
        }

        if (restartButton == null)
        {
            Debug.LogError("El campo restartButton no está asignado en el UIManager.");
        }
        else
        {
            restartButton.onClick.AddListener(RestartGame);
            restartButton.gameObject.SetActive(false);
        }
        // Mostrar el título inicial y el botón "Start"
        if (titleText != null)
        {
            titleText.SetActive(true);
        }
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartGame);


    }

    private void Update()
    {
        if (!gameStarted)
        {
            // Rotar el titleText
            if (titleText != null)
            {
                RectTransform titleRectTransform = titleText.GetComponent<RectTransform>();
                titleRectTransform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
            }
        }

        if (isCountdownRunning && !isGameOver)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                isCountdownRunning = false;
                GameOver();
            }
            UpdateTimerText();
        }
        if (startButton.gameObject.activeSelf)
        {
            // Deshabilitar el movimiento de los jugadores
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController player in players)
            {
                player.enabled = false;
                isCountdownRunning = false;
            }
        }
        if (!startButton.gameObject.activeSelf && !isGameOver)
        {
            // Deshabilitar el movimiento de los jugadores
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            foreach (PlayerController player in players)
            {
                player.enabled = true;
                isCountdownRunning = true;
            }
        }
        
    }

    // ABSTRACTION
    public void UpdateCounterText(int count)
    {
        if (counterText != null)
        {
            counterText.text = "Score: " + count;
        }
    }

    // ABSTRACTION
    public void StartCountdown()
    {
        timer = countdownDuration;
        isCountdownRunning = true;
    }

    // ABSTRACTION
    public void UpdateTimerText()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timer);
            string timeString = string.Format("{0:00}''", seconds);
            timerText.text = "Timer: " + timeString;
        }
    }

    // ABSTRACTION
    private void GameOver()
    {
        isGameOver = true;
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);

        }

        // Desactivar el movimiento de los "Player"
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in players)
        {
            player.enabled = false;
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }
        MainManager.Instance.SaveData(Counter.globalCount);

    }

    // ABSTRACTION
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Counter.RestartGlobalCount();
                
    }

    // ABSTRACTION
    public void StartGame()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            titleText.SetActive(false);
            startButton.gameObject.SetActive(false);
            isCountdownRunning = true;
            StartCountdown();

        }
    }
}