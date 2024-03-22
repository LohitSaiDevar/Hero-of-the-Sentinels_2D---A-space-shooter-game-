using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Drone : MonoBehaviour
{
    [SerializeField] GameObject lasersPrefab;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float rotateSpeed = 5;

    [SerializeField] int maxHealth = 50;
    [SerializeField] int points;

    PlayerController player;

    [SerializeField] HealthBar healthBar;
    bool laserActive;
    float currentHealth;
    [SerializeField] int laserTimer = 3;
    GameManager gameManager;

    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip laserSound;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        ToggleLaser();
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.right, Space.World);
        transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
        if (transform.position.x <= -21.5f)
        {
            // Change direction to move right
            moveSpeed = Mathf.Abs(moveSpeed);
        }
        else if (transform.position.x >= 21.5f)
        {
            // Change direction to move left
            moveSpeed = -Mathf.Abs(moveSpeed);
        }
    }
    void ToggleLaser()
    {
        if (!laserActive)
        {
            
            StartCoroutine(LaserSpawn(laserTimer));
        }
    }
    IEnumerator LaserSpawn(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        if (!laserActive)
        {
            // Activate the laser
            laserActive = true;

            // Play the laser sound
            GetComponent<AudioSource>().PlayOneShot(laserSound, 0.1f);

            // Activate the laser prefab
            lasersPrefab.SetActive(true);

            // Wait for the cooldown time again before deactivating the laser
            yield return new WaitForSeconds(coolDownTime);

            // Deactivate the laser
            laserActive = false;

            // Deactivate the laser prefab
            lasersPrefab.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_Player"))
        {
            Destroy(collision.gameObject);
            HandleCollisionDamage(player.attackDamage, gameObject);
        }
        else if (collision.gameObject.CompareTag("Player_Laser"))
        {
            HandleCollisionDamage(player.attackDamage + 30, gameObject);
        }
        else if (collision.gameObject.CompareTag("Player_GruntBullet"))
        {
            HandleCollisionDamage(player.attackDamage + 2, gameObject);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player_EliteBullet"))
        {
            HandleCollisionDamage(player.attackDamage + 4, gameObject);
            Destroy(collision.gameObject);
        }
    }
    void TakingDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Drone's Health: " + currentHealth);
    }

    void HandleCollisionDamage(float damage, GameObject gameObject)
    {
        TakingDamage(damage);
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            PlayExplosion();
            gameManager.killCount += 1;
        }
    }
    void PlayExplosion()
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
    }
}
