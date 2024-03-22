using System;
using System.Collections;
using System.Drawing;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 5;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float spawnBulletTime;
    [SerializeField] float spawnBreakTime;
    GameManager gameManager;
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] int points;
    GameObject bossUI;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] ParticleSystem explosion;
    PlayerController player;
    HealthBar_Player healthBar;

    [SerializeField] GameObject homingMissilePrefab;
    bool bulletSpawnedLeft;
    bool bulletSpawnedRight;
    float healthPercent;

    EnemyLaser enemyLaser;
    bool isLaserEnabled;
    bool isPhase3;
    [SerializeField] int laserTimer;
    [SerializeField] GameObject lasersPrefab;
    private void Awake()
    {
        bossUI = GameObject.Find("Enemy_Boss_UI");
        healthBar = bossUI.GetComponentInChildren<HealthBar_Player>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(SpawnBulletRepeat());
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        enemyLaser = GetComponentInChildren<EnemyLaser>();
    }

    private void Update()
    {
        if (isPhase3)
        {
            transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
            ToggleLaser();
        }
        healthPercent = currentHealth / maxHealth;
    }
    void SpawnBullets()
    {
        float radius = 2;
        int numberOfBullets = 16;
        float anglePerBullet = 360 / numberOfBullets;
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * anglePerBullet;
            float xPos = transform.position.x + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float zPos = transform.position.z + radius * Mathf.Cos(Mathf.Deg2Rad * angle);

            Vector3 spawnPosition = new Vector3(xPos, 2.8f, zPos);
            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            bulletInstance.transform.Rotate(0, angle, 0);
        }
    }

    void SpawnHomingBullets()
    {
        float radius = 2.5f;
        if (!bulletSpawnedLeft)
        {
            float leftAngle = 90f;
            float leftXPos = transform.position.x + radius * Mathf.Sin(Mathf.Deg2Rad * leftAngle);
            float leftZPos = transform.position.z + radius * Mathf.Cos(Mathf.Deg2Rad * leftAngle);
            Vector3 leftSpawnPosition = new Vector3(leftXPos, 2.8f, leftZPos);
            GameObject bulletInstance = Instantiate(homingMissilePrefab, leftSpawnPosition, Quaternion.identity);
            bulletInstance.transform.Rotate(0, leftAngle, 0);
            bulletSpawnedLeft = true;
        }
        if (!bulletSpawnedRight)
        {
            float rightAngle = 270f;
            float rightXPos = transform.position.x + radius * Mathf.Sin(Mathf.Deg2Rad * rightAngle);
            float rightZPos = transform.position.z + radius * Mathf.Cos(Mathf.Deg2Rad * rightAngle);
            Vector3 rightSpawnPosition = new Vector3(rightXPos, 2.8f, rightZPos);
            GameObject bulletInstance = Instantiate(homingMissilePrefab, rightSpawnPosition, Quaternion.identity);
            bulletInstance.transform.Rotate(0, rightAngle, 0);
            bulletSpawnedRight = true;
        }
    }
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
            CollisionDamage(collision, 0);
        }
        if (collision.gameObject.CompareTag("Player_GruntBullet"))
        {
            CollisionDamage(collision, 2);
        }
        if (collision.gameObject.CompareTag("Player_EliteBullet"))
        {
            CollisionDamage(collision, 4);
        }
        if (collision.gameObject.CompareTag("Player_Laser"))
        {
            TakingDamage(0);
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                gameManager.UpdateScore(points);
            }
        }
    }
    void TakingDamage(float damage)
    {
        currentHealth -= damage;
    }

    void CollisionDamage(Collision collision, int extraDmg)
    {
        TakingDamage(player.attackDamage + extraDmg);
        healthBar.SetHealth(currentHealth);
        Destroy(collision.gameObject);
        Debug.Log("Health Percent: " + healthPercent);

        if (currentHealth <= 0)
        {
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
            bossUI.SetActive(false);
            gameManager.UpdateScore(points);
            gameManager.GameCompleted();
        }
        else if (healthPercent <= 0.5f && healthPercent > 0.3f)
        {
            SpawnHomingBullets();
        }
        else if (healthPercent <= 0.3f && healthPercent > 0)
        {
            isPhase3 = true;
        }
    }
    IEnumerator LaserSpawn(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        isLaserEnabled = true;
        lasersPrefab.SetActive(true);
        yield return new WaitForSeconds(coolDownTime);
        isLaserEnabled = false;
        lasersPrefab.SetActive(false);
    }
    void ToggleLaser()
    {
        if (!isLaserEnabled)
        {
            StartCoroutine(LaserSpawn(laserTimer));
        }
    }
}
