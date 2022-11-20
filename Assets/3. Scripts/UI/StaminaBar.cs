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

    public HorizontalLayoutGroup hlg;
    int padding;

    void SetBars()
    {
        foreach(GameObject b in barObject)
        {
            Destroy(b);
        }

        bars.Clear();

        padding = 85;

        for(int i = 0; i < soPlayer.soPlayerMove.maxStaminas; i++) {
            GameObject staminaSlot = Instantiate(staminas, this.transform) as GameObject;
            
            barObject.Add(staminaSlot);

            bars.Add(staminaSlot.GetComponent<StaminaImage>().bar);

            padding -= 85;
        }

        hlg.padding.left = padding;
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

