using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    GameManager gameManager;
    // Update is called once per frame

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.down);
        if (this.transform.position.z < -40)
        {
            Destroy(gameObject);
            gameManager.starsSpawned = false;
        }
        if (this.transform.position.z < -10.6f && !gameManager.starsSpawned)
        {
            gameManager.SpawnStars();
            gameManager.starsSpawned = true;
        }
    }
}
