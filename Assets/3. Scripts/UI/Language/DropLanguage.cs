using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropLanguage : MonoBehaviour
{
    public SOConfig soConfig;

    public string[] text;
    public string[] textEnglish;

    private int index;
    void Start()
    {
        if (textEnglish.Length != text.Length) textEnglish = new string[text.Length];

        for(int i = 0; i < text.Length; i++)
        {
            if(soConfig.language == 0)
            {
                GetComponent<TMP_Dropdown>().options[i].text = text[i];
            }
            else if(soConfig.language == 1)
            {
                GetComponent<TMP_Dropdown>().options[i].text = textEnglish[i];
            }
            
        }
    }

}
