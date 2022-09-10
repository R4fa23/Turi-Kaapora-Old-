using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTrigger : MonoBehaviour
{
    public SOSave soSave;
    public Transform savePoint;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soSave.EnterSave(savePoint);
        }
    }
}
