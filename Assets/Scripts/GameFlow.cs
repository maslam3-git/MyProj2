
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance { get; private set; }

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text heightText;
    [SerializeField] private TMP_Text bestHeightText;
    [SerializeField] private Transform player;

    private float startY;
    private int bestHeight;

    private bool ended;
    private bool paused;
    private bool endlessMode;

    public bool IsEnded => ended;
    public bool IsEndlessMode => endlessMode;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        startY = player.position.y;

        bestHeight = PlayerPrefs.GetInt(
            "LavaRushBestHeight", 0
        );

        UpdateHeight(0);
    }

    private void Update()
    {
        if (ended)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (paused)
                Resume();
            else
                Pause();
        }

        if (paused)
            return;

        int height = Mathf.Max(
            0,
            Mathf.FloorToInt(player.position.y - startY)
        );

        UpdateHeight(height);
    }

    private void UpdateHeight(int height)
    {
        if (height > bestHeight)
        {
            bestHeight = height;

            PlayerPrefs.SetInt(
                "LavaRushBestHeight",
                bestHeight
            );

            PlayerPrefs.Save();
        }

        heightText.text = "Height: " + height + " m";
        bestHeightText.text = "Best: " + bestHeight + " m";
    }

    public void Lose()
    {
        if (ended)
            return;

        ended = true;
        paused = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayLose();

        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Win()
    {
        if (ended || endlessMode)
            return;

        ended = true;
        paused = false;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ContinueEndless()
    {
        endlessMode = true;
        ended = false;
        paused = false;

        PortalExit[] portals =
            FindObjectsByType<PortalExit>(
                FindObjectsSortMode.None
            );

        foreach (PortalExit portal in portals)
        {
            portal.gameObject.SetActive(false);
            Destroy(portal.gameObject);
        }

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);

        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
            AudioManager.Instance.ResumeMusic();

        Debug.Log("Endless Mode started!");
    }

    public void Pause()
    {
        if (ended || paused)
            return;

        paused = true;

        pausePanel.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PauseMusic();

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!paused)
            return;

        paused = false;

        pausePanel.SetActive(false);

        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
            AudioManager.Instance.ResumeMusic();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Caves");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }
}
