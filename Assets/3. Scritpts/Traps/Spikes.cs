using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public SOPlayer soPlayer;
    public GameObject spikes;
    public int damage;
    public  float cooldownSpike;
    bool inSpace;
    public float timeToSpike;
    float currentTime;
    bool spiking;
    void Start()
    {
        currentTime = 0;
        spikes.SetActive(false);
    }

    
    void Update()
    {
        if(soPlayer.state == SOPlayer.State.DASHING || !inSpace)
        {
            currentTime -= Time.deltaTime;
            if(currentTime < 0) currentTime = 0;
        }
        else if(inSpace)
        {
            currentTime += Time.deltaTime;
        }

        if(currentTime >= timeToSpike && !spiking) ActiveSpike();

        
    }

    void ActiveSpike()
    {
        spiking = true;
        spikes.SetActive(true);
        soPlayer.soPlayerHealth.HealthChange(-damage);
        StartCoroutine(DisableSpikes());
    }

    IEnumerator DisableSpikes()
    {
        yield return new WaitForSeconds(cooldownSpike);
        spiking = false;
        spikes.SetActive(false);
        currentTime = 0;

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inSpace = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
        {
            inSpace = false;
        }
    }
}
