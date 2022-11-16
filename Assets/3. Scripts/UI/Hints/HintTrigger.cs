using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintTrigger : MonoBehaviour
{
    GameObject panel;

    TextMeshProUGUI text;

    public string description;

    private void Start()
    {
        panel = GameObject.FindGameObjectWithTag("HintCanvas").GetComponent<HintPanel>().panel;
        text = GameObject.FindGameObjectWithTag("HintCanvas").GetComponent<HintPanel>().text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            panel.SetActive(true);
            text.text = description;
            gameObject.SetActive(false);
        }
    }
}
