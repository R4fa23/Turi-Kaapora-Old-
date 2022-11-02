using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public SOPlayer soPlayer;
    [SerializeField] GameObject staminas;
    [HideInInspector] public List<Image> bars; 

    [HideInInspector] public List<GameObject> barObject;
    float currentStamina;
    float lastStamina;

    void SetBars()
    {
        foreach(GameObject b in barObject)
        {
            Destroy(b);
        }

        bars.Clear();

        for(int i = 0; i < soPlayer.soPlayerMove.maxStaminas; i++) {
            GameObject staminaSlot = Instantiate(staminas, this.transform) as GameObject;
            
            barObject.Add(staminaSlot);

            bars.Add(staminaSlot.GetComponent<StaminaImage>().bar);
        }
    }

    void OnEnable()
    {
        soPlayer.soPlayerMove.ChangeMaxStaminaEvent.AddListener(SetBars);
    }

    void OnDisable()
    {
        soPlayer.soPlayerMove.ChangeMaxStaminaEvent.RemoveListener(SetBars);
    }
}

