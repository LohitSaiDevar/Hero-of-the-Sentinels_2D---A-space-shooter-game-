using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    public float speed;
    Rigidbody rb;
    GameManager gameManager;
    private bool isShooting;

    public HealthBar_Player healthBar;
    public float maxHealth = 20;
    int currentExp = 0, minExp = 0, maxExp = 20;
    public float currentHealth;
    public float attackDamage;

    //PowerAbsorber
    [SerializeField] PowerAbsorber powerAbsorber;
    bool paCooldownActive;
    public float paCDTime;
    public float paMaxDmg;
    [SerializeField] TextMeshProUGUI paCoolDownText;
    public float absorbedDmg;
    [SerializeField] PowerUps powerUps;

    [SerializeField] ExperienceBar expBar;
    public int currentLvl = 1;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] Upgrade upgrade;

    CommandInvoker commandInvoker;

    [SerializeField] ParticleSystem explosion;

    [SerializeField] AudioClip bulletSound;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip damageSound;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log("Current Health: " + currentHealth);
        Debug.Log("Current lvl: " + currentLvl);
        Debug.Log("Min exp: " + minExp);
        expBar.SetMinExp(currentExp);
        expBar.GetComponent<Slider>().maxValue = maxExp;
        commandInvoker = GetComponent<CommandInvoker>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    //When player takes damage, then it's health reduces.
    public void TakingDamage(int damage)
    {
        currentHealth -= damage;
        audioSource.PlayOneShot(damageSound, 0.1f);
    }

    //To toggle Power Absorber On/Off
    void TogglePowerAbsorber()
    {
        //Input
        bool powerAbsorberActive = Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1);
        //Set active
        if (powerAbsorberActive && !powerAbsorber.gameObject.activeSelf && absorbedDmg <= paMaxDmg)
        {
            powerAbsorber.SetActiveState(true);
            if (paCooldownActive)
            {
                StartCoroutine(PACoolDown(paCDTime));
            }
        }
        // Check if power absorber was previously active and is now inactive
        else if (!powerAbsorberActive && powerAbsorber.gameObject.activeSelf)
        {
            powerAbsorber.SetActiveState(false);
        }
        // Check if Cooldown is active
        else if (absorbedDmg >= paMaxDmg)
        {
            paCooldownActive = true;
        }
        //If Cooldown is active, then Start Coroutine
        if (paCooldownActive)
        {
            StartCoroutine(PACoolDown(paCDTime));
        }
    }

    IEnumerator PACoolDown(float count)
    {
        powerAbsorber.SetActiveState(false);
        paCoolDownText.gameObject.SetActive(true);
        while (count > 0)
        {
            yield return new WaitForSeconds(1);
            count--;
        }
        absorbedDmg = 0;
        paCoolDownText.gameObject.SetActive(false);
        paCooldownActive = false;
    }

    public void HandleExperienceChange(int newExp)
    {
        currentExp += newExp;
        Debug.Log("Current exp: " + currentExp);
        expBar.SetExp(currentExp);
        if (currentExp >= maxExp)
        {
            LevelUp();
            maxExp *= 2;
            expBar.GetComponent<Slider>().maxValue = maxExp;
        }
    }

    private void LevelUp()
    {
        minExp = 0;
        expBar.SetMinExp(minExp);
        currentLvl++;
        gameManager.LevelUpScreenOn();
        upgrade.UpgradeSpaceship(currentLvl);
        lvlText.text = "LVL: " + currentLvl;
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
