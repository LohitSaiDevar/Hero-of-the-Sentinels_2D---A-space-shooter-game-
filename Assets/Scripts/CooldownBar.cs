using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CooldownBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMaxCooldown(float cdtime)
    {
        slider.maxValue = cdtime;
        slider.value = cdtime;
        fill.color = gradient.Evaluate(1);
    }
    public void SetCooldown(float cdtime)
    {
        slider.value = cdtime;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
