using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    PlayerController player;
    GameManager gameManager;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            player.PlayExplosion();
            gameManager.GameOver();
        }
    }
}
