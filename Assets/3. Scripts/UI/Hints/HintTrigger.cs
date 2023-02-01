using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintTrigger : MonoBehaviour
{
    public SOConfig soConfig;

    GameObject panel;

    TextMeshProUGUI text;

    Image image;

    HintPanel hintPanel;

    [TextArea(15, 20)]
    public string description;
    
    [TextArea(15, 20)]
    public string descriptionEnglish;

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
            TriggerEvents();
        }
    }

    public void TriggerEvents()
    {
        Debug.Log("Hint");
        panel.SetActive(true);
        if(soConfig.language == 0)text.text = description;
        else if(soConfig.language == 1) text.text = descriptionEnglish;
        image.sprite = illustration;
        gameObject.SetActive(false);
    }
}
