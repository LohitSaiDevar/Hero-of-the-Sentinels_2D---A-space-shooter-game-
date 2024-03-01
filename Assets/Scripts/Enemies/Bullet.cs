using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    PlayerController player;
    GameManager gameManager;
    [SerializeField] int damage;

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
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
        if (transform.position.z > 22 || transform.position.z < -20)
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
    }
}
