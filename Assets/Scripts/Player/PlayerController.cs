using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    private bool isShooting;

    public HealthBar_Player healthBar;
    [SerializeField] int maxHealth = 20;
    int currentExp = 0, minExp = 0, maxExp = 100;
    public int currentHealth;
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

    [SerializeField] ParticleSystem explosion;

    [SerializeField] AudioClip bulletSound;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip damageSound;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
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
        //Movement boundary
        Zbound();
        Xbound();

        //Toggle PowerAbsorber On/Off
        TogglePowerAbsorber();

        //Weapons management
        Weapons();
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = GetMouseDirection();
        MouseMovement(moveDirection);
    }
    void KeyboardMovement(float horizontalInput, float verticalInput)
    {
        Vector3 movement = new(horizontalInput, 0, verticalInput);
        movement.Normalize();
        rb.velocity = movement * speed;
    }
    Vector3 GetMouseDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, transform.position.z));
        return (targetPos - transform.position).normalized;
    }
    void MouseMovement(Vector3 moveDirection)
    {
        rb.velocity = moveDirection * speed;
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
        if (Input.GetMouseButtonDown(0)) // Check for initial left mouse button press
        {
            isShooting = true; // Start shooting
        }
        else if (Input.GetMouseButtonUp(0)) // Check for left mouse button release
        {
            isShooting = false; // Stop shooting
        }
        if ((isShooting || Input.GetKey(KeyCode.Space)) && !powerUps.countdown)
        {
            audioSource.PlayOneShot(bulletSound, 0.002f);
            audioSource.pitch = 3;
            ICommand shootCommand = new ShootCommand(powerUps, transform);
            commandInvoker.SetShootCommand(shootCommand);
            commandInvoker.ExecuteShootCommand();
        }
        else
        {
            if (powerUps != null)
                powerUps.laserActive = false;
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
    }

    //When player takes damage, then it's health reduces.
    public void TakingDamage(int damage)
    {
        currentHealth -= damage;
        audioSource.PlayOneShot(damageSound, 0.1f);
    }

    //To toggle Power Absorber On/Off
    void TogglePowerAbsorber()
    {
        bool powerAbsorberActive = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
        if (powerAbsorberActive && !powerAbsorber.gameObject.activeSelf)
        {
            powerAbsorber.SetActiveState(true);
        }
        // Check if power absorber was previously active and is now inactive
        else if (!powerAbsorberActive && powerAbsorber.gameObject.activeSelf)
        {
            powerAbsorber.SetActiveState(false);
        }
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

    public void SetHealth(int currentHealth)
    {
        healthBar.SetHealth(currentHealth);
    }

    public void PlayExplosion()
    {
        ParticleSystem explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.GetComponent<ParticleSystem>().Play();
        explosionInstance.GetComponent<AudioSource>().PlayOneShot(explosionSound, 0.3f);
    }
}
