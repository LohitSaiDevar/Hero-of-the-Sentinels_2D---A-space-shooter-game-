using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject starsPrefab;
    public GameObject enemyGruntPrefab;
    public GameObject eliteGruntPrefab;
    public GameObject dronePrefab;
    
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;

    [SerializeField] TextMeshProUGUI finalScoreText;

    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] Button homeButton;
    [SerializeField] Button restartButton;

    [SerializeField] Button playButton;
    [SerializeField] Button howToPlayButton;
    [SerializeField] Image menuBg;

    public TextMeshProUGUI titleText; // Reference to the TextMeshPro component
    public float transitionDuration = 0.5f; // Duration of the color transition in seconds

    bool bossReady;

    void Start()
    {
        SpawnStars();
        SpawnEnemyGrunt();
    }

    void SpawnStars()
    {
        Vector3 spawnStarPos = new(0, 2.5f, 40);
        Instantiate(starsPrefab, spawnStarPos, Quaternion.Euler(90,0,0));
    }
    public void SpawnEnemyGrunt()
    {
        if (!bossReady)
        {
            Vector3 spawnGruntPos = new(-20, 3, Random.Range(4, 9));
            int randomNum1 = Random.Range(0, 2);
            if (randomNum1 == 0)
            {
                spawnGruntPos.x = -spawnGruntPos.x;
            }
            Instantiate(enemyGruntPrefab, spawnGruntPos, Quaternion.Euler(0, 180, 0));
        }
    }
    public void SpawnEliteGrunt()
    {
        if (!bossReady)
        {
            Vector3 spawnElitePos = new(20, 3, Random.Range(4, 9));
            int randomNum2 = Random.Range(0, 2);
            if (randomNum2 == 0)
            {
                spawnElitePos.x = -spawnElitePos.x;
            }
            Instantiate(eliteGruntPrefab, spawnElitePos, Quaternion.Euler(0, 180, 0));
        }
    }
    public void SpawnDrone()
    {
        if (!bossReady)
        {
            Vector3 spawnDronePos = new(-20, 3, Random.Range(4, 9));
            int randomNum3 = Random.Range(0, 2);
            if (randomNum3 == 0)
            {
                spawnDronePos.x = -spawnDronePos.x;
            }
            Instantiate(dronePrefab, spawnDronePos, Quaternion.Euler(0, 180, 0));
        }
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
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void UpdateScore(int point)
    {
        score += point;
        scoreText.text = "Score: " + score;

        if (score == 20)
        {
            SpawnEliteGrunt();
        }
        else if (score <= 50 && score >= 40)
        {
            SpawnDrone();
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
