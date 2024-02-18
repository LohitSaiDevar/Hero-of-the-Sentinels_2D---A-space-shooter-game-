using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static Action<int> OnExperienceChange;

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }
}
