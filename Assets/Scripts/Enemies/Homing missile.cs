using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homingmissile : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;
    PlayerController player;
    GameManager gameManager;
    [SerializeField] int damage;

    Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (transform.position.z > 22 || transform.position.z < -20 || transform.position.x > 19 || transform.position.x < -19)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.forward).y;

        if (transform.position.z - player.transform.position.z > 3)
        {
            rb.angularVelocity = new Vector3(0, -rotateAmount * rotateSpeed, 0);
        }
        rb.velocity = transform.forward * moveSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakingDamage(damage);
                player.healthBar.SetHealth(player.currentHealth);
                Destroy(this.gameObject);
                if (player.currentHealth <= 0)
                {
                    Destroy(other.gameObject);
                    player.PlayExplosion();
                    gameManager.GameOver();
                }
            }
        }
    }
}
