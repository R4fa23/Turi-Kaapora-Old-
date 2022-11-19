using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestFPS : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Start()
    {
        //Application.targetFrameRate = Screen.currentResolution.refreshRate;
        text.text = QualitySettings.vSyncCount.ToString();
    }
    public void Test(bool t)
    {
        if (t) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        
    }
    private void Update()
    {
        text.text = QualitySettings.vSyncCount.ToString();
    }
}
