using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOConfig", menuName = "ScriptableObjects/Config")]
public class SOConfig : ScriptableObject
{
    public int rate;
    public int resolution;
    public int quality;
    public bool fullscreen;
    public bool vsync;

    public bool firstTime;

    public int language;

    /*
     * Portugu�s = 0
     * Ingl�s = 1
     */
}
