using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public SOPlayer soPlayer;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(-damage);
        }
    }
}
