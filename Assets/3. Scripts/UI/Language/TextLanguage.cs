using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLanguage : MonoBehaviour
{
    public SOConfig soConfig;

    public string[] texts;
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = texts[soConfig.language];
    }
}
