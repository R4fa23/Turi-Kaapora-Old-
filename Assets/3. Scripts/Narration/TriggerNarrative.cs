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

    public float[] talkTime;
    int index;
    TextMeshProUGUI talk;
    bool talking;

    float narrationVel;

    void Start()
    {
        talk = GameObject.FindGameObjectWithTag("TextNarrative").GetComponent<TextMeshProUGUI>();
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
        talk.gameObject.SetActive(true);

        FMODUnity.RuntimeManager.PlayOneShot("event:/" + evento[index], transform.position);

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
            FMODUnity.RuntimeManager.PlayOneShot("event:/" + evento[index], transform.position);
            talk.text = falas[index];

            float timeToPass;
            //timeToPass = ((falas[index].Length / 4.5f) * narrationVel);
            timeToPass = talkTime[index];
            StartCoroutine(WaitNextTalk(timeToPass));
        }
    }
}
