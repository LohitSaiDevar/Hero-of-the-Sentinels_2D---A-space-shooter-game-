using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;

    [SerializeField] HealthBar_Player healthBar;
    [SerializeField] int maxHealth = 20;
    int currentHealth, currentExp = 0, minExp = 0, maxExp = 100;
    public int attackDamage;

    GameManager gameManager;

    [SerializeField] PowerAbsorber powerAbsorber;
    [SerializeField] PowerUps powerUps;

    [SerializeField] ExperienceBar expBar;
    public int currentLvl = 1;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] Upgrade upgrade;

    CommandInvoker commandInvoker;
    float horizontalInput;
    float verticalInput;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log("Current Health: " + currentHealth);
        Debug.Log("Current lvl: " + currentLvl);
        Debug.Log("Min exp: " + minExp);
        expBar.SetMinExp(currentExp);
        expBar.GetComponent<Slider>().maxValue = maxExp;
        commandInvoker = GetComponent<CommandInvoker>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Movement(horizontalInput, verticalInput, transform);
        //Movement boundary
        Zbound();
        Xbound();

        //Toggle PowerAbsorber On/Off
        TogglePowerAbsorber();

        //Weapons management
        Weapons();
    }
    void Movement(float horizontalInput, float verticalInput, Transform transform)
    {
        ICommand moveCommand = new MoveCommand(transform, speed, horizontalInput, verticalInput);
        commandInvoker.SetMoveCommand(moveCommand);
        commandInvoker.ExecuteMoveCommand();
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
            ICommand shootCommand = new ShootCommand(powerUps, transform);
            commandInvoker.SetShootCommand(shootCommand);
            commandInvoker.ExecuteShootCommand();
        }

        if (powerUps.changeToLaser && !powerUps.laserCooldown)
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

    public void HandleExperienceChange(int newExp)
    {
        currentExp += newExp;
        Debug.Log("Current exp: " + currentExp);
        expBar.SetExp(currentExp);
        if (currentExp >= maxExp)
        {
            LevelUp();
            maxExp += 100;
            expBar.GetComponent<Slider>().maxValue = maxExp;
        }
    }

    private void LevelUp()
    {
        minExp += 100;
        Debug.Log("Min exp: " + minExp);
        expBar.SetMinExp(minExp);
        maxHealth += 10;
        attackDamage += 3;
        currentLvl++;
        upgrade.UpgradeSpaceship(currentLvl);
        lvlText.text = "LVL: " + currentLvl;
        Debug.Log("Level Up, Lvl: " + currentLvl);
    }

    private void OnEnable()
    {
        ExperienceManager.OnExperienceChange += HandleExperienceChange;
    }
}
