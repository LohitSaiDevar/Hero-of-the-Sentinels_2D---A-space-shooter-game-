using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float spawnBulletTime;
    [SerializeField] float spawnBreakTime;
    GameManager gameManager;
    [SerializeField] int maxHealth = 80;
    int currentHealth;
    [SerializeField] int points;
    GameObject bossUI;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] ParticleSystem explosion;
    PlayerController player;
    HealthBar_Player healthBar;
    private void Awake()
    {
        bossUI = GameObject.Find("Enemy_Boss_UI");
        healthBar = bossUI.GetComponentInChildren<HealthBar_Player>();
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
            Debug.Log("Enemy boss Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
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
        if (transform.position.z != 4)
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    void CollisionDamage(Collision collision, int extraDmg)
    {
        TakingDamage(player.attackDamage + extraDmg);
        healthBar.SetHealth(currentHealth);
        Destroy(collision.gameObject);
        Debug.Log("Enemy boss Health: " + currentHealth);
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
        }
    }
}
