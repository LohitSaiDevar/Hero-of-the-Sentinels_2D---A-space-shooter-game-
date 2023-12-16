using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int currentHealth;
    [SerializeField] int laserTimer = 3;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        
        currentHealth = maxHealth;
        
        healthBar.SetMaxHealth(maxHealth);
    }
    void Update()
    {
        ToggleLaser();
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.right, Space.World);
        transform.Rotate(new Vector3(0,rotateSpeed,0) * Time.deltaTime);
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
        laserActive = true;
        lasersPrefab.SetActive(true);
        yield return new WaitForSeconds(coolDownTime);
        laserActive = false;
        lasersPrefab.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet_player"))
        {
            TakingDamage(player.attackDamage);
            healthBar.SetHealth(currentHealth);
            Destroy(collision.gameObject);
            Debug.Log("Current Drone Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Player_Laser"))
        {
            TakingDamage(6);
            healthBar.SetHealth(currentHealth);
            Debug.Log("Current Drone Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    void TakingDamage(int damage)
    {
        currentHealth -= damage;
    }
}
