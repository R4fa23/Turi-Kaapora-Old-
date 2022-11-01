using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    [HideInInspector] public SOPlayer soPlayer;
    [SerializeField] GameObject staminas;

    void Start()
    {
        for(int i = 0; i < soPlayer.soPlayerMove.maxStaminas; i++) {
            GameObject staminaSlot = Instantiate(staminas, this.transform) as GameObject;
        }
    }
}
