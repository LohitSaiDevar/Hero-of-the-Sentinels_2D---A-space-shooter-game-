using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] Sprite mediumSprite;  // Reference to the 'Medium' sprite
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component on the 'Sprite_Player ' game object
        spriteRenderer = GameObject.Find("Sprite_Player").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Call a method to handle the spaceship upgrade
            UpgradeSpaceship();
        }
    }

    void UpgradeSpaceship()
    {
        // Change the sprite to the 'Medium' sprite
        if (mediumSprite != null)
        {
            spriteRenderer.sprite = mediumSprite;
        }
        else
        {
            Debug.LogError("Medium sprite not assigned in the inspector.");
        }
    }
}
