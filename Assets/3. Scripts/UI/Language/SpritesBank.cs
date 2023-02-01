using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritesBank : MonoBehaviour
{
    public SOConfig soConfig;

    public Sprite[] sprites;
    void Start()
    {
        GetComponent<Image>().sprite = sprites[soConfig.language];
    }
}
