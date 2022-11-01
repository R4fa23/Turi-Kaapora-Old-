using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FortFire : MonoBehaviour
{
    public VisualEffect boitataFire;

    public void ResetToRedFire()
    {
        boitataFire.SetFloat("Percent", 0);
    }

    public void SetToBlueFire()
    {
        boitataFire.SetFloat("Percent", 1);
    }

    public void AddBlueFire(float addPercent)
    {
        float currentAmount = boitataFire.GetFloat("Percent");
        currentAmount += addPercent;
        if(currentAmount > 1) currentAmount = 1;
        boitataFire.SetFloat("Percent", currentAmount);
    }
}
