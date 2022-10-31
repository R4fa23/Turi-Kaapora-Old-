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
    int index;
    public TextMeshProUGUI talk;

    void Start()
    {
        talk = GameObject.FindGameObjectWithTag("TextNarrative").GetComponent<TextMeshProUGUI>();
        talk.gameObject.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartTalks();
        }
    }

    public void StartTalks()
    {
        talk.gameObject.SetActive(true);

        FMODUnity.RuntimeManager.PlayOneShot("event:" + evento[index], transform.position);

        talk.text = falas[index];

        float timeToPass;
        timeToPass = ((falas[index].Length / 4.5f) * 2);
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
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:" + evento[index], transform.position);
            talk.text = falas[index];

            float timeToPass;
            timeToPass = ((falas[index].Length / 4.5f) * 2);
            StartCoroutine(WaitNextTalk(timeToPass));
        }
    }
}
