using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float spawnBulletTime;
    [SerializeField] float spawnBreakTime;

    [SerializeField] HealthBar_Player healthBar;
    GameManager gameManager;
    [SerializeField] int maxHealth = 80;
    int currentHealth;
    [SerializeField] int points;

    PlayerController player;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(SpawnBulletRepeat());
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void SpawnBullets()
    {
        Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -2), transform.rotation);
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
            Debug.Log("Enemy boss Health: " + currentHealth);
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
            Debug.Log("Enemy boss Health: " + currentHealth);
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
            Debug.Log("Enemy boss Health: " + currentHealth);
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
            TakingDamage(0);
            healthBar.SetHealth(currentHealth);
            Debug.Log("Enemy boss Health: " + currentHealth);
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
}
