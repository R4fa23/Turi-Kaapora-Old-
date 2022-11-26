using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerNarrative : MonoBehaviour
{
    public String[] evento;

    public String[] falas;

    public FMOD.Studio.EventInstance instanciaEvento;

    public float[] talkTime;
    int index;
    TextMeshProUGUI talk;
    bool talking;

    float narrationVel;

    public SOPlayer soPlayer;

    void Start()
    {
        talk = GameObject.FindGameObjectWithTag("TextNarrative").GetComponent<CollectText>().text;
        talk.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!talking)
            {
                talking = true;
                StartTalks();
            }
            
        }
    }

    public void StartTalks()
    {
        Debug.Log("start talk");
        talk.gameObject.SetActive(true);


        //FMODUnity.RuntimeManager.PlayOneShot("event:/" + evento[index], transform.position);
        instanciaEvento = FMODUnity.RuntimeManager.CreateInstance("event:/" + evento[index]);
        instanciaEvento.start();

        talk.text = falas[index];

        float timeToPass;
        //timeToPass = ((falas[index].Length / 4.5f) * narrationVel);
        timeToPass = talkTime[index];
        StartCoroutine(WaitNextTalk(timeToPass));

    }

    IEnumerator WaitNextTalk(float time)
    {
        yield return new WaitForSeconds(time);
        NextTalk();
    }

    void NextTalk()
    {
        index++;
        if(index >= falas.Length)
        {
            talk.text = "";
            talk.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
        else
        {
            //FMODUnity.RuntimeManager.PlayOneShot("event:/" + evento[index], transform.position);
            instanciaEvento = FMODUnity.RuntimeManager.CreateInstance("event:/" + evento[index]);
            instanciaEvento.start();

            talk.text = falas[index];

            float timeToPass;
            //timeToPass = ((falas[index].Length / 4.5f) * narrationVel);
            timeToPass = talkTime[index];
            StartCoroutine(WaitNextTalk(timeToPass));
        }
    }

    void Pause()
    {
        if(talking)
        {
            instanciaEvento.setPaused(true);
        }
    }

    void Unpause()
    {
        if(talking)
        {
            instanciaEvento.setPaused(false);
        }
    }

    private void OnEnable()
    {
        soPlayer.PauseEvent.AddListener(Pause);
        soPlayer.UnpauseEvent.AddListener(Unpause);
    }

    private void OnDisable()
    {
        soPlayer.PauseEvent.RemoveListener(Pause);
        soPlayer.UnpauseEvent.RemoveListener(Unpause);
    }
}
