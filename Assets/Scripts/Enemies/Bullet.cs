using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    PlayerController player;
    GameManager gameManager;
    [SerializeField] int damage;
    PowerUps powerUps;
    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }

        GameObject gameManagerObject = GameObject.Find("GameManager");
        if (gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager GameObject not found!");
        }
        powerUps = playerObject.GetComponent<PowerUps>();
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        if (transform.position.z > 22 || transform.position.z < -20 || transform.position.x > 19 || transform.position.x < -19)
        {
            Destroy(gameObject);
        }
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
        else if (other.gameObject.CompareTag("PowerAbsorber"))
        {
            if (gameObject.CompareTag("Bullet"))
            {
                Debug.Log("Changed to GruntBullet");
                powerUps.BulletGruntActive();
                Destroy(this.gameObject);
            }
            else if (gameObject.CompareTag("BulletElite"))
            {
                Debug.Log("Changed to elite");
                powerUps.BulletEliteActive();
                Destroy(this.gameObject);
            }
        }
    }
}
