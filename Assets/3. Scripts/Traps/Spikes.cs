using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
    public GameObject spikes;
    public  float cooldownDownSpike;
    bool inSpace;
    public float timeToUp;
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

        if(currentTime >= timeToUp && !spiking) ActiveSpike();

        
    }

    void ActiveSpike()
    {
        spiking = true;
        spikes.SetActive(true);
        StartCoroutine(DisableSpikes());
    }

    IEnumerator DisableSpikes()
    {
        yield return new WaitForSeconds(cooldownDownSpike);
        spiking = false;
        spikes.SetActive(false);
        currentTime = 0;

    }

    void Restart()
    {
        StopAllCoroutines();
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

    void OnEnable()
    {
        soSave.RestartEvent.AddListener(Restart);
    }
    void OnDisable()
    {
        soSave.RestartEvent.RemoveListener(Restart);
    }
}
