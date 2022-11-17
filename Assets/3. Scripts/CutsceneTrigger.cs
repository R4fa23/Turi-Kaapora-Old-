using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneTrigger : MonoBehaviour
{
    public UnityEvent Triggerer;
    private BoxCollider collide;

    private void Start()
    {
        collide = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Triggerer?.Invoke();
            //gameObject.SetActive(false);
            collide.enabled = false;
        }
    }
}
