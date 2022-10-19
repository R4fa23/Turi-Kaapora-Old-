using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    
        InvokeRepeating(nameof(CalculateFps), 0, 1);
    }

    void CalculateFps()
    {
        text.text = (1 / Time.deltaTime).ToString("0") + " FPS";
    }
}
