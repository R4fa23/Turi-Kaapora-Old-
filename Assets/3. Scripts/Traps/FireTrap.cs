using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    public SOPlayer soPlayer;
    bool fired;
    
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!fired)
            {
                soPlayer.soPlayerHealth.Burned(3);
                fired = true;
                StartCoroutine(CooldownFire());
            }
            
        }
    }

    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(0.5f);
        fired = false;
    }
}
