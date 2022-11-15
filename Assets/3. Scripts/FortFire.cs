using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class FortFire : MonoBehaviour
{
    public VisualEffect boitataFire;
    [SerializeField] Volume blueFire;

    [HideInInspector]
    public float quantityToAdd;

    float currentAmount;
    float targetAmount;

	private void Update()
	{        
        if (currentAmount != targetAmount)
        {
            currentAmount = Mathf.MoveTowards(currentAmount, targetAmount, 0.0005f);
            if (currentAmount > 1)
            {
                currentAmount = 1;
                targetAmount = 1;
            }
            boitataFire.SetFloat("Percent", currentAmount);
            blueFire.weight = currentAmount;
        }        
	}
	public void ResetToRedFire()
    {
        boitataFire.SetFloat("Percent", 0);
        blueFire.weight = 0;
        currentAmount = 0;
        targetAmount = 0;
    }

    public void SetToBlueFire()
    {
        boitataFire.SetFloat("Percent", 1);
        blueFire.weight = 1;
        currentAmount = 1;
        targetAmount = 1;
    }

    public void AddBlueFire(float addPercent)
    {
        targetAmount += addPercent;
        /*float currentAmount = boitataFire.GetFloat("Percent");
        currentAmount += addPercent;
        if(currentAmount > 1) currentAmount = 1;
        boitataFire.SetFloat("Percent", currentAmount);*/        
    }

    public void AddQuantity()
    {
        AddBlueFire(quantityToAdd);
    }
}
