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
    [SerializeField] SOPlayer soPlayer;

    [HideInInspector]
    public float quantityToAdd;

    float currentAmount;
    float targetAmount;

    float currentAmountClip;
    float targetAmountClip;


    public bool endFire;

	private void Update()
	{        
        if (currentAmount != targetAmount)
        {
            currentAmount = Mathf.MoveTowards(currentAmount, targetAmount, 1f * Time.deltaTime);
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

    public void EndFire()
    {
        StartCoroutine(LevelUp());
    }

    IEnumerator LevelUp()
    {
        boitataFire.SendEvent("End");
        boitataFire.SendEvent("StartDash");
        yield return new WaitForSeconds(7f);
        boitataFire.SendEvent("EndDash");
    }

    /*private void OnEnable()
    {
        soPlayer.LevelUpEvent.AddListener(EndFire);
    }

    private void OnDisable()
    {
        soPlayer.LevelUpEvent.RemoveListener(EndFire);
    }*/
}
