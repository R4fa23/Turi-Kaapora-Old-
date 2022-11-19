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
}
