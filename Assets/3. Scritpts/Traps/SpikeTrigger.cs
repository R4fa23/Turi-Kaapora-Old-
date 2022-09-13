using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    public SOPlayer soPlayer;
    public int damage;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(-damage);
        }
    }
}
