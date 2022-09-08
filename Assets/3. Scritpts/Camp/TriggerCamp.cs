using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCamp : MonoBehaviour
{
    public Camp manager;
    public SOCamp soCamp;
    void Start()
    {
        soCamp = manager.soCamp;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soCamp.EnterCamp();
        }
    }
}
