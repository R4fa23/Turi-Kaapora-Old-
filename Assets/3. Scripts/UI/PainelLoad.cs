using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainelLoad : MonoBehaviour
{
    public GameObject painel;

    public CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup.alpha = 1.0f;
    }
}
