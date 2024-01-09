using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    [SerializeField] float speed;

    [SerializeField] HealthBar_Player healthBar;
    [SerializeField] int maxHealth = 20;
    int currentHealth;
    public int attackDamage;

    GameManager gameManager;

    [SerializeField] PowerAbsorber powerAbsorber;
    [SerializeField] PowerUps powerUps;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log("Current Health: " + currentHealth);
    }
    void Update()
    {
        //Player movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(speed * Time.deltaTime * verticalInput * Vector3.forward);
        transform.Translate(speed * Time.deltaTime * horizontalInput * Vector3.right);

        //Movement boundary
        Zbound();
        Xbound();

        //Toggle PowerAbsorber On/Off
        TogglePowerAbsorber();

        //Weapons management
        Weapons();
        if (Input.GetKey(KeyCode.Space) && powerUps.changeToLaser && !powerUps.defaultWeapon &&
       !powerUps.changeToBulletGrunt && !powerUps.changeToBulletElite && !powerUps.laserCooldown)
        {
            powerUps.laserActive = true;
            powerUps.ToggleLaser();

            if (!powerUps.laserCooldown && !powerUps.isCooldownActive)
            {
                powerUps.StartCoroutine(powerUps.LaserCooldown());
            }
        }
        else
        {
            powerUps.laserActive = false;
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

    void Weapons()
    {
        if (Input.GetKey(KeyCode.Space) && !powerUps.countdown)
        {
            if (powerUps.defaultWeapon && !powerUps.changeToBulletGrunt && !powerUps.changeToLaser && !powerUps.changeToBulletElite)
            {
                powerUps.ShootDefaultWeapon(transform);
            }
            else if (powerUps.changeToBulletGrunt && !powerUps.defaultWeapon && !powerUps.changeToBulletElite && !powerUps.changeToLaser)
            {
                powerUps.ShootBulletGrunt(transform);
            }
            else if (powerUps.changeToBulletElite && !powerUps.defaultWeapon && !powerUps.changeToBulletGrunt && !powerUps.changeToLaser)
            {
                powerUps.ShootBulletElite(transform);
            }
        }

        if (powerUps.changeToLaser && !powerUps.defaultWeapon && !powerUps.changeToBulletGrunt && !powerUps.changeToBulletElite && !powerUps.laserCooldown)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                powerUps.laserActive = true;
                powerUps.ToggleLaser();

                if (!powerUps.laserCooldown && !powerUps.isCooldownActive)
                {
                    powerUps.StartCoroutine(powerUps.LaserCooldown());
                }
            }
            else
            {
                powerUps.laserActive = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (powerAbsorber.gameObject.activeSelf)//When bullets/laser from the enemies collide on Player's power absorber
        {
            Destroy(collision.gameObject);
            if (collision.gameObject.CompareTag("Bullet"))
            {
                powerUps.BulletGruntActive();
            }
            else if (collision.gameObject.CompareTag("BulletElite"))
            {
                powerUps.BulletEliteActive();
            }
            else if (collision.gameObject.CompareTag("Laser"))
            {
                powerUps.LaserActive();
            }
            return;
        }

        if (collision.gameObject.CompareTag("Bullet"))//When Grunt's bullet collides with player
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
        else if (collision.gameObject.CompareTag("BulletElite"))//When Elite grunt's bullet collides with player
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
        else if (collision.gameObject.CompareTag("Laser"))//When Drone's laser collides with the player
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

    //When player takes damage, then it's health reduces.
    void TakingDamage(int damage)
    {
        currentHealth -= damage;
    }

    //To toggle Power Absorber On/Off
    void TogglePowerAbsorber()
    {
        bool powerAbsorberActive = Input.GetKey(KeyCode.LeftShift);
        powerAbsorber.SetActiveState(powerAbsorberActive);

        // If power absorber is active, return early to avoid further collision handling
        if (powerAbsorberActive)
            return;
    }
}
