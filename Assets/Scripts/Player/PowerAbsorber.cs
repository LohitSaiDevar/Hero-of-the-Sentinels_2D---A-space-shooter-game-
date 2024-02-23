using UnityEngine;

public class PowerAbsorber : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip sound;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SetActiveState(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive && audioSource != null && sound != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(sound, 0.3f);
        }
    }
}
