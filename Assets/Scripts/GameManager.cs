using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int killCount;
    [SerializeField] GameObject starsPrefab, enemyGruntPrefab, eliteGruntPrefab, dronePrefab, bossPrefab;
    [SerializeField] GameObject warningTextUI;

    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    int highScore;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI thankYouText;
    [SerializeField] TextMeshProUGUI completedDemoText;
    [SerializeField] Button homeButton;
    [SerializeField] Button restartButton;

    [SerializeField] Button playButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Image menuBg;

    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject howToPlayMenuUI;

    public TextMeshProUGUI titleText; // Reference to the TextMeshPro component
    public float transitionDuration = 0.5f; // Duration of the color transition in seconds

    public bool bossReady = false;
    [SerializeField] GameObject bossUI;
    bool bossSpawned;

    public static bool IsGamePaused = false;

    public bool keyboardOnly;
    public bool keyboardAndMouse;
    public bool mouseOnly;
    [SerializeField] TextMeshProUGUI controls;
    [SerializeField] GameObject controlsUI;

    //To check if Gameobject controlsUI is active or not
    bool controlsUIActive;
    [SerializeField] GameObject paExplanationUI;
    [SerializeField] GameObject paExplanationButton;

    bool paExplanationUIActive;

    public bool starsSpawned;

    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip congratulationsSound;
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip warningSound;
    AudioSource audioSource;

    [SerializeField] GameObject player;
    [SerializeField] GameObject mainCamera;
    AudioSource mainAudioSource;
    bool droneSpawned;

    public bool isGameOver;
    void Start()
    {
        mainAudioSource = mainCamera.GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        SpawnEnemyGrunt();
        SpawnEnemyGrunt();
        SpawnEnemyGrunt();
        SpawnEnemyGrunt();
        SpawnEnemyGrunt();

        LoadControlLayout();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void SpawnStars()
    {
        Vector3 spawnStarPos = new(0, 2.5f, 30.5f);
        Instantiate(starsPrefab, spawnStarPos, Quaternion.Euler(90, 0, 0));
    }

    public void SpawnEnemyGrunt()
    {
        Vector3 spawnGruntPos = new(-20, 3, Random.Range(4, 9));
        int randomNum1 = Random.Range(0, 2);
        if (randomNum1 == 0)
        {
            spawnGruntPos.x = -spawnGruntPos.x;
        }
        Instantiate(enemyGruntPrefab, spawnGruntPos, Quaternion.Euler(0, 180, 0));
    }
    public void SpawnEliteGrunt()
    {
        Vector3 spawnElitePos = new(20, 3, Random.Range(4, 9));
        int randomNum2 = Random.Range(0, 2);
        if (randomNum2 == 0)
        {
            spawnElitePos.x = -spawnElitePos.x;
        }
        Instantiate(eliteGruntPrefab, spawnElitePos, Quaternion.Euler(0, 180, 0));
    }
    public void SpawnDrone()
    {
        Vector3 spawnDronePos = new(-20, 3, Random.Range(4, 9));
        int randomNum3 = Random.Range(0, 2);
        if (randomNum3 == 0)
        {
            spawnDronePos.x = -spawnDronePos.x;
        }
        Instantiate(dronePrefab, spawnDronePos, Quaternion.Euler(0, 180, 0));
    }

    public void SpawnBoss()
    {
        bossUI.SetActive(true);
        mainAudioSource.PlayOneShot(bossMusic, 0.5f);
        Vector3 spawnBossPos = new(0, 3, 22);
        Instantiate(bossPrefab, spawnBossPos, Quaternion.Euler(0, 180, 0));
    }

    IEnumerator BossArrives()
    {
        bossReady = true;
        int count = 5;
        mainAudioSource.PlayOneShot(warningSound, 0.3f);
        while (count > 0)
        {
            warningTextUI.SetActive(true);
            yield return new WaitForSeconds(1);
            count--;
        }
        warningTextUI.SetActive(false);
        yield return StartCoroutine(SpawnBossAsync());
    }

    IEnumerator SpawnBossAsync()
    {
        // Start a custom profiler marker
        UnityEngine.Profiling.Profiler.BeginSample("SpawnBoss");

        // Spawn the boss asynchronously
        SpawnBoss();

        // End the custom profiler marker
        UnityEngine.Profiling.Profiler.EndSample();

        // Wait until the next frame before continuing
        yield return null;
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void HowToPlayMenu()
    {
        mainMenuUI.SetActive(false);
        howToPlayMenuUI.SetActive(true);
        player.SetActive(false);
    }
    public void ReturnMainMenu()
    {
        isGameOver = false;
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        bossReady = false;
        mainAudioSource.Stop();
    }
    public void HowToPlayReturnMainMenu()
    {
        howToPlayMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
        player.SetActive(true);
    }
    public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        bossReady = false;
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }
    public void UpdateScore(int point)
    {
        score += point;
        scoreText.text = "Score: " + score;

        if (killCount == 25 && !bossReady)
        {
            bossReady = true; // Set bossReady to true when conditions are met
            StartCoroutine(BossArrives());
        }
        else
        {
            // Continue with the existing logic for spawning enemies based on score
            // This ensures that enemies keep spawning until the boss appears
            if (!bossReady)
            {
                if (!droneSpawned && (score > 100 && score < 190))
                {
                    SpawnDrone();
                    droneSpawned = true; // Set the flag to true to indicate that the drone has been spawned
                }
                else if (droneSpawned && (score > 200 && score < 290))
                {
                    droneSpawned = false;
                }
                else if (!droneSpawned && score > 300)
                {
                    SpawnDrone();
                    droneSpawned = true;
                }
                else
                {
                    // Continue with the existing logic for spawning enemies based on score
                    if (score == 10)
                    {
                        SpawnEnemyGrunt();
                    }
                    else if (score == 30)
                    {
                        SpawnEnemyGrunt();
                    }
                    else if (score == 50)
                    {
                        SpawnEliteGrunt();
                    }
                }
            }
        }

    }
    public void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        finalScoreText.text = "Score: " + score;
        finalScoreText.gameObject.SetActive(true);

        LoadHighScore();
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore(highScore);
        }
        highScoreText.text = "High Score: " + highScore;
        highScoreText.gameObject.SetActive(true);
        audioSource.PlayOneShot(gameOverSound, 0.3f);
        bossReady = false;
        mainAudioSource.Stop();
    }

    public void GameCompleted()
    {
        isGameOver = true;
        thankYouText.gameObject.SetActive(true);
        completedDemoText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        finalScoreText.text = "Score: " + score;
        finalScoreText.gameObject.SetActive(true);

        LoadHighScore();
        if (score > highScore)
        {
            highScore = score;
            SaveHighScore(highScore);
        }
        highScoreText.text = "High Score: " + highScore;
        highScoreText.gameObject.SetActive(true);
        audioSource.PlayOneShot(congratulationsSound, 0.3f);
        bossReady = false;
        mainAudioSource.Stop();
    }

    public void SetKeyboardOnly()
    {
        keyboardOnly = true;
        keyboardAndMouse = false;
        mouseOnly = false;
        controls.text = ": Keyboard Only";
        SaveControlLayout(": Keyboard Only");
    }

    public void SetKeyboardAndMouse()
    {
        keyboardAndMouse = true;
        mouseOnly = false;
        keyboardOnly = false;
        controls.text = ": Keyboard & Mouse";
        SaveControlLayout(": Keyboard & Mouse");
    }

    public void SetMouseOnly()
    {
        mouseOnly = true;
        keyboardAndMouse = false; 
        keyboardOnly = false;
        controls.text = ": Mouse Only";
        SaveControlLayout(": Mouse Only");
    }

    public void OpenControlsPanel()
    {
        if (!controlsUIActive)
        {
            controlsUIActive = true;
            controlsUI.SetActive(controlsUIActive);
            paExplanationUI.SetActive(false);
            paExplanationButton.SetActive(!controlsUIActive);
        }
        else
        {
            controlsUIActive = false;
            controlsUI.SetActive(controlsUIActive);
            paExplanationButton.SetActive(!controlsUIActive);
        }
    }
    public void OpenPAInstructionsPanel()
    {
        if (!paExplanationUIActive)
        {
            paExplanationUIActive = true;
            paExplanationUI.SetActive(paExplanationUIActive);
            controlsUI.SetActive(false);
        }
        else
        {
            paExplanationUIActive = false;
            paExplanationUI.SetActive(paExplanationUIActive);
        }
    }
    void SaveControlLayout(string layout)
    {
        PlayerPrefs.SetString("ControlLayout", layout);
    }

    // Load the last selected control layout from PlayerPrefs
    void LoadControlLayout()
    {
        string layout = PlayerPrefs.GetString("ControlLayout", "Keyboard Only"); // Default to "Keyboard Only" if not found
        controls.text = layout;
    }
    void SaveHighScore(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highScore = savedHighScore;
    }
}
