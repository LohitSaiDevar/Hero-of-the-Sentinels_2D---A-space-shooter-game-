using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    GameManager gameManager;
    // Update is called once per frame

    bool starsSpawned = false;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.down);
        if (transform.position.z < -40)
        {
            Destroy(gameObject);
            starsSpawned = false;
        }
        Debug.Log("Position: " + transform.position.z);
        if (transform.position.z < -10.6f && !starsSpawned)
        {
            gameManager.SpawnStars();
            Debug.Log("Stars Spawned!");
            starsSpawned = true;
        }
    }
}
