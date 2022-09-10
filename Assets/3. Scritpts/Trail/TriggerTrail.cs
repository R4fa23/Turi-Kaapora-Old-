using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrail : MonoBehaviour
{
    public Trail manager;

    [HideInInspector]
    public SOTrail soTrail;
    void Start()
    {
        soTrail = manager.soTrail;;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soTrail.EnterTrial();
            this.gameObject.SetActive(false);
        }
    }
}
