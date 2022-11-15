using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelVFX : MonoBehaviour
{
    [SerializeField] VisualEffect[] vfxs;

    public void UpdateVFXLevel(int vfxLevel)
    {
        foreach (var effect in vfxs)
        {
            effect.SetInt("Level", vfxLevel);
        }
    }

}
