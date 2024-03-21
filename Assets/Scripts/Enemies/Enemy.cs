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
    [SerializeField] int expPoints;

    PlayerController player;
    ExperienceManager expManager;
    [SerializeField] ParticleSystem explosion;

    [SerializeField] AudioClip explosionSound;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        expManager = GameObject.Find("ExperienceManager").GetComponent<ExperienceManager>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (gameObject != null)
        {
            StartCoroutine(SpawnBulletRepeat());
        }
    }
    private void FixedUpdate()
    {
        GruntMovement();
    }
    private void LateUpdate()
    {
        EliteMovement();
    }
    void SpawnBullets()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -1), transform.rotation);
    }

    //After every 0.3s, spawn bullet continuously.
    //After 2 secs, bullets take a break for 1.5 secs.

    IEnumerator SpawnBulletRepeat()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!gameManager.isGameOver)
                {
                    SpawnBullets();
                }
                yield return new WaitForSeconds(spawnBulletTime);
            }
            yield return new WaitForSeconds(spawnBreakTime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_Player"))
        {
            HandleBulletCollision(player.attackDamage);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player_GruntBullet"))
        {
            HandleBulletCollision(player.attackDamage + 2);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player_EliteBullet"))
        {
            HandleBulletCollision(player.attackDamage + 4);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player_Laser"))
        {
            HandleBulletCollision(maxHealth);
            Destroy(collision.gameObject);
        }
    }
    void TakingDamage(int damage)
    {
        currentHealth -= damage;
    }

    void GruntMovement()
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
        else
        {
            return;
        }
    }
    void EliteMovement()
    {
        if (gameObject.CompareTag("Elite"))
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) > 0)
            {
                if (transform.position.x < player.transform.position.x)
                {
                    transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);
                }
                else
                {
                    transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
                }
            }
        }
        else
        {
            return;
        }
    }

    void HandleBulletCollision(int damage)
    {
        TakingDamage(damage);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            DestroyEnemy();
        }
    }

    //The explosion plays here
    void DestroyEnemy()
    {
        gameManager.killCount += 1;
        // Instantiate the explosion effect prefab at the position of the enemy
        ParticleSystem explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.GetComponent<ParticleSystem>().Play();
        if (explosionInstance.GetComponent<AudioSource>() != null)
        {
            explosionInstance.GetComponent<AudioSource>().PlayOneShot(explosionSound, 0.3f);
        }
        else
        {
            Debug.Log("Audio Source is missing!");
        }
        Destroy(gameObject);
        if (gameObject.CompareTag("Grunt"))
        {
            if (!gameManager.bossReady)
            {
                gameManager.SpawnEnemyGrunt();
            }
            expManager.AddExperience(expPoints);
        }
        else if (gameObject.CompareTag("Elite"))
        {
            if (!gameManager.bossReady)
            {
                gameManager.SpawnEliteGrunt();
            }
            Debug.Log("Wave 1 completed!");
            expManager.AddExperience(expPoints);
        }
        gameManager.UpdateScore(points);
    }
}
