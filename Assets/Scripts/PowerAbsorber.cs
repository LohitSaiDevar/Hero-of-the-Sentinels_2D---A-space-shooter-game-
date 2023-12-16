using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAbsorber : MonoBehaviour
{
    public void SetActiveState(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
