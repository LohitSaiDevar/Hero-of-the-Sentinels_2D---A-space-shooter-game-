using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] Sprite lvl2Sprite;
    [SerializeField] Sprite lvl3Sprite;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] PlayerController player;

    void Start()
    {
        // Get the SpriteRenderer component on the 'Sprite_Player ' game object
        spriteRenderer = GameObject.Find("Sprite_Player").GetComponent<SpriteRenderer>();
    }

    public void UpgradeSpaceship(int lvl)
    {
        // Change the sprite to the 'Medium' sprite
        if (lvl == 3)
        {
            player.speed += 3;
            if (lvl2Sprite != null)
            {
                spriteRenderer.sprite = lvl2Sprite;
            }
            else
            {
                Debug.LogError("Medium sprite not assigned in the inspector.");
            }
        }
        else if(lvl == 6)
        {
            player.speed += 3;
            if (lvl3Sprite != null)
            {
                spriteRenderer.sprite = lvl3Sprite;
            }
            else
            {
                Debug.LogError("Medium sprite not assigned in the inspector.");
            }
        }
        else
        {
            return;
        }
    }
}
