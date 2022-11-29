using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public SOPlayer soPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(15);
            gameObject.SetActive(false);
        }
    }
}
