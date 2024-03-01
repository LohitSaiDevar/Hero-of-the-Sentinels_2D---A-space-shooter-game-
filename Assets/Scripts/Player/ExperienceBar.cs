using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Color color;
    [SerializeField] Image fill;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMinExp(int exp)
    {
        slider.minValue = exp;
        slider.value = exp;
        if (fill != null)
        {
            fill.color = Color.clear;
        }
    }

    public void SetExp(int exp)
    {
        slider.value = exp;
        if (fill != null)
        {
            fill.color = Color.yellow;
        }
    }
}

