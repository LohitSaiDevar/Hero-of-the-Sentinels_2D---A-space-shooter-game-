using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.down);
        if (transform.position.z < -40)
        {
            Destroy(gameObject);
        }
    }
}
