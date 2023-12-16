using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] Vector3 offsetPos;

    void Start()
    {
        slider = GetComponent<Slider>();
    }
    private void Update()
    {
        transform.SetPositionAndRotation(target.transform.position + offsetPos, cam.transform.rotation);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
