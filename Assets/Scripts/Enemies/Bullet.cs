using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);

        if (transform.position.z > 22 || transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
}
