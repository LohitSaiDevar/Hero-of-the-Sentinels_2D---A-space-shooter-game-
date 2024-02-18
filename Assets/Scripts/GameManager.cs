using System.Collections;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject starsPrefab, enemyGruntPrefab, eliteGruntPrefab, dronePrefab, bossPrefab;

    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    [SerializeField] TextMeshProUGUI finalScoreText;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] Button homeButton;
    [SerializeField] Button restartButton;

    [SerializeField] Button playButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Image menuBg;

    [SerializeField] GameObject pauseMenuUI;

    public TextMeshProUGUI titleText; // Reference to the TextMeshPro component
    public float transitionDuration = 0.5f; // Duration of the color transition in seconds

    public static bool BossReady = false;
    [SerializeField] GameObject bossUI;

    public static bool IsGamePaused = false;
    bool starsSpawned;

    void Start()
    {
        SpawnEnemyGrunt();
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
        Debug.Log("Position: " + starsPrefab.transform.position.z);
        if (starsPrefab.transform.position.z < -10.6f && !starsSpawned)
        {
            SpawnStars();
            Debug.Log("Stars Spawned!");
            starsSpawned = true; // Set the flag to true to indicate that stars have been spawned
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
        Vector3 spawnBossPos = new(0, 3, 22);
        Instantiate(bossPrefab, spawnBossPos, Quaternion.Euler(0, 180, 0));
    }

    IEnumerator TransitionGradientColors()
    {
        while (true)
        {
            // Randomize the four corner gradient colors
            Color targetTopLeftColor = RandomColor();
            Color targetTopRightColor = RandomColor();
            Color targetBottomLeftColor = RandomColor();
            Color targetBottomRightColor = RandomColor();

            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                // Interpolate between the current colors and the target colors
                float t = elapsedTime / transitionDuration;
                Color interpolatedTopLeftColor = Color.Lerp(titleText.colorGradient.topLeft, targetTopLeftColor, t);
                Color interpolatedTopRightColor = Color.Lerp(titleText.colorGradient.topRight, targetTopRightColor, t);
                Color interpolatedBottomLeftColor = Color.Lerp(titleText.colorGradient.bottomLeft, targetBottomLeftColor, t);
                Color interpolatedBottomRightColor = Color.Lerp(titleText.colorGradient.bottomRight, targetBottomRightColor, t);

                // Apply the interpolated colors to the four corner gradient
                titleText.colorGradient = new VertexGradient(
                    interpolatedTopLeftColor,
                    interpolatedTopRightColor,
                    interpolatedBottomLeftColor,
                    interpolatedBottomRightColor
                );
                yield return null;
                // Update elapsed time
                elapsedTime += Time.deltaTime;
            }
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
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

        //Wave 1
        if (!BossReady)
        {
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
        finalScoreText.text = "Final Score: " + score;
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(true);
    }
    Color RandomColor()
    {
        // Generate a random color
        return new Color(Random.value, Random.value, Random.value);
    }
}
