using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed;

    [SerializeField] List<GameObject> weapons;
    bool countdown;

    [SerializeField] HealthBar_Player healthBar;
    [SerializeField] int maxHealth = 20;
    int currentHealth;
    public int attackDamage;

    GameManager gameManager;

    [SerializeField] PowerAbsorber powerAbsorber;
    public bool changeToBulletGrunt;
    public bool changeToBulletElite;
    public bool changeToLaser;
    public bool defaultWeapon;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] bool laserActive;
    public float laserCDTime = 0;
    bool laserCooldown;
    int laserCDMaxTime = 5;
    bool isCooldownActive = false;

    [SerializeField] CooldownBar laserCDBar;
    private void Start()
    {
        laserCDBar.SetMaxCooldown(laserCDMaxTime);
        laserActive = false;
        defaultWeapon = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log("Current Health: "+ currentHealth);
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(speed * Time.deltaTime * verticalInput * Vector3.forward);
        transform.Translate(speed * Time.deltaTime * horizontalInput * Vector3.right);

        Zbound();
        Xbound();
        TogglePowerAbsorber();
        if (Input.GetKey(KeyCode.Space) && !countdown)
        {
            if (defaultWeapon && !changeToBulletGrunt && !changeToLaser && !changeToBulletElite)
            {
                Instantiate(weapons[0], transform.position + new Vector3(0, 0, 1), transform.rotation);
                countdown = true;
                StartCoroutine(CountDown());
            }
            else if (changeToBulletGrunt && !defaultWeapon && !changeToBulletElite && !changeToLaser)
            {
                Instantiate(weapons[1], transform.position + new Vector3(0, 0, 1), transform.rotation);
                countdown = true;
                StartCoroutine(CountDown());
            }
            else if (changeToBulletElite && !defaultWeapon && !changeToBulletGrunt && !changeToLaser)
            {
                Instantiate(weapons[2], transform.position + new Vector3(0, 0, 1), transform.rotation);
                countdown = true;
                StartCoroutine(CountDown());
            }
        }
        if (changeToLaser && !defaultWeapon && !changeToBulletGrunt && !changeToBulletElite && !laserCooldown)
        {
            ToggleLaser();
        }
        if (laserCooldown && !isCooldownActive)
        {
            StartCoroutine(LaserCooldown());
        }
    }
    void Zbound()
    {
        if (transform.position.z > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if (transform.position.z < -9)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -9);
        }
    }
    void Xbound()
    {
        if (transform.position.x > 16.5f)
        {
            transform.position = new Vector3(16.5f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -16.5f)
        {
            transform.position = new Vector3(-16.5f, transform.position.y, transform.position.z);
        }
    }
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.1f);
        countdown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (powerAbsorber.gameObject.activeSelf)
        {
            Destroy(collision.gameObject);
            if (collision.gameObject.CompareTag("Bullet"))
            {
                defaultWeapon = false;
                changeToBulletGrunt = true;
                changeToBulletElite = false;
                changeToLaser = false;
            }
            else if (collision.gameObject.CompareTag("BulletElite"))
            {
                changeToBulletGrunt = false;
                changeToBulletElite = true;
                defaultWeapon = false;
                changeToLaser = false;
            }
            else if (collision.gameObject.CompareTag("Laser"))
            {
                defaultWeapon = false;
                changeToBulletGrunt = false;
                changeToBulletElite = false;
                changeToLaser = true;
            }
            return;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            TakingDamage(3);
            healthBar.SetHealth(currentHealth);

            Destroy(collision.gameObject);
            Debug.Log("Taking Damage");
            Debug.Log("current Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
        else if (collision.gameObject.CompareTag("BulletElite"))
        {
            TakingDamage(5);
            healthBar.SetHealth(currentHealth);

            Destroy(collision.gameObject);
            Debug.Log("Taking Damage");
            Debug.Log("current Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
        else if (collision.gameObject.CompareTag("Laser"))
        {
            TakingDamage(maxHealth);
            healthBar.SetHealth(currentHealth);
            Debug.Log("Taking Damage");
            Debug.Log("current Health: " + currentHealth);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                gameManager.GameOver();
            }
        }
    }

    void TakingDamage(int damage)
    {
        currentHealth -= damage;
    }

    void TogglePowerAbsorber()
    {
        bool powerAbsorberActive = Input.GetKey(KeyCode.LeftShift);
        powerAbsorber.SetActiveState(powerAbsorberActive);

        // If power absorber is active, return early to avoid further collision handling
        if (powerAbsorberActive)
            return;
    }
    
    void ToggleLaser()
    {
        laserActive = Input.GetKey(KeyCode.Space);
        laserPrefab.SetActive(laserActive);
        if (laserActive && laserCDTime <= laserCDMaxTime)
        {
            laserCDTime += Time.deltaTime;
            laserCDBar.SetCooldown(laserCDTime);
            laserCDBar.gameObject.SetActive(true);
        }
        else if (!laserActive && laserCDTime > 0)
        {
            laserCDTime = Mathf.Max(0, laserCDTime - Time.deltaTime);
            laserCDBar.SetCooldown(laserCDTime);
            if (laserCDTime == 0)
            {
                laserCDBar.gameObject.SetActive(false);
            }
        }
        if (laserCDTime >= laserCDMaxTime)
        {
            laserCooldown = true;
        }
    }
    IEnumerator LaserCooldown()
    {
        isCooldownActive = true;
        laserPrefab.SetActive(false);

        while (laserCDTime > 0)
        {
            laserCDTime = Mathf.Max(0, laserCDTime - Time.deltaTime);
            laserCDBar.SetCooldown(laserCDTime);
            yield return null;
        }

        laserCDBar.gameObject.SetActive(false);
        isCooldownActive = false;
        laserCooldown = false;
    }
}
