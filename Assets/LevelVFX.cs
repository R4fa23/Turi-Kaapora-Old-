using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelVFX : MonoBehaviour
{
    [SerializeField] VisualEffect[] vfxs;

    public SOPlayer soPlayer;

    public void UpdateVFXLevel()
    {
        int vfxLevel = soPlayer.level + 1;

        foreach (var effect in vfxs)
        {
            effect.SetInt("Level", vfxLevel);
        }
    }

    private void OnEnable()
    {
        soPlayer.LevelUpEvent.AddListener(UpdateVFXLevel);
    }

    private void OnDisable()
    {
        soPlayer.LevelUpEvent.RemoveListener(UpdateVFXLevel);
    }

}
