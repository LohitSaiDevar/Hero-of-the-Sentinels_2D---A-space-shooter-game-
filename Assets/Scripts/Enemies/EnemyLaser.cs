using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    PlayerController player;
    GameManager gameManager;
    PowerUps powerUps;
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        powerUps = player.gameObject.GetComponent<PowerUps>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            player.PlayExplosion();
            gameManager.GameOver();
        }
        else if (other.gameObject.CompareTag("PowerAbsorber"))
        {
            if (player != null)
            {
                Destroy(this.gameObject);
                powerUps.BulletGruntActive();
            }
        }
    }
}
