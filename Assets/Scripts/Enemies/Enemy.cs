using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float spawnBulletTime;   // Time for continuous bullet firing
    [SerializeField] float spawnBreakTime;    // Break time between bullet waves 

    [SerializeField] HealthBar healthBar;
    GameManager gameManager;
    [SerializeField] int maxHealth = 15;
    int currentHealth;
    [SerializeField] int points;

    PlayerController player;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(SpawnBulletRepeat());
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private void Update()
    {
        Movement();
    }
    //After every 0.3s, spawn bullet continuously.
    //After 2 secs, bullets take a break for 1.5 secs.
    void SpawnBullets()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -1), transform.rotation);
    }

    IEnumerator SpawnBulletRepeat()
    {
        float currentTotalTime = 1;
        float currentTime = spawnBulletTime;
        while (currentTotalTime > 0)
        {
            currentTotalTime -= Time.deltaTime;
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                SpawnBullets();
                currentTime = spawnBulletTime;
            }
            yield return null;
        }
        yield return new WaitForSeconds(spawnBreakTime);
        StartCoroutine(SpawnBulletRepeat());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_player"))
        {
            TakingDamage(player.attackDamage);
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
            Debug.Log("current enemy Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                if (gameObject.CompareTag("Grunt"))
                    gameManager.SpawnEnemyGrunt();
                else if (gameObject.CompareTag("Elite"))
                    gameManager.SpawnEliteGrunt();
                gameManager.UpdateScore(points);
            }
        }
        if (collision.gameObject.CompareTag("Player_GruntBullet"))
        {
            TakingDamage(5);
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
            Debug.Log("current enemy Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                if (gameObject.CompareTag("Grunt"))
                    gameManager.SpawnEnemyGrunt();
                else if (gameObject.CompareTag("Elite"))
                    gameManager.SpawnEliteGrunt();
                gameManager.UpdateScore(points);
            }
        }
        if (collision.gameObject.CompareTag("Player_EliteBullet"))
        {
            TakingDamage(7);
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
            Debug.Log("current enemy Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                if (gameObject.CompareTag("Grunt"))
                    gameManager.SpawnEnemyGrunt();
                else if (gameObject.CompareTag("Elite"))
                    gameManager.SpawnEliteGrunt();
                gameManager.UpdateScore(points);
            }
        }
        if (collision.gameObject.CompareTag("Player_Laser"))
        {
            TakingDamage(maxHealth);
            healthBar.SetHealth(currentHealth);
            Debug.Log("current enemy Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                if (gameObject.CompareTag("Grunt"))
                    gameManager.SpawnEnemyGrunt();
                else if (gameObject.CompareTag("Elite"))
                    gameManager.SpawnEliteGrunt();
                gameManager.UpdateScore(points);
            }
        }
    }
    void TakingDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Movement()
    {
        if (gameObject.CompareTag("Grunt"))
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);
            if (transform.position.x <= -16.5f)
            {
                // Change direction to move right
                moveSpeed = Mathf.Abs(moveSpeed);
            }
            else if (transform.position.x >= 16.5f)
            {
                // Change direction to move left
                moveSpeed = -Mathf.Abs(moveSpeed);
            }

        }
        else if (gameObject.CompareTag("Elite"))
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
            if (transform.position.x <= -16.5f)
            {
                // Change direction to move right
                moveSpeed = -Mathf.Abs(moveSpeed);
            }
            else if (transform.position.x >= 16.5f)
            {
                // Change direction to move left
                moveSpeed = Mathf.Abs(moveSpeed);
            }
        }
    }
}
