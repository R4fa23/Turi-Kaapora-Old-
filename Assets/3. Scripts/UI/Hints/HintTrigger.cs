using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintTrigger : MonoBehaviour
{
    GameObject panel;

    TextMeshProUGUI text;

    Image image;

    HintPanel hintPanel;

    public string description;

    public Sprite illustration;

    private void Start()
    {
        hintPanel = GameObject.FindGameObjectWithTag("HintCanvas").GetComponent<HintPanel>();
        panel = hintPanel.panel;
        text = hintPanel.text;
        image = hintPanel.image;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            panel.SetActive(true);
            text.text = description;
            image.sprite = illustration;
            gameObject.SetActive(false);
        }
    }
}
