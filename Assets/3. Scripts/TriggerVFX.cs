using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TriggerVFX : MonoBehaviour
{
    public SOPlayer soPlayer;
    public VisualEffect[] vfx;
    //public string eventToTrigger;

    public void VFXEvent(string trigger)
    {
        for (int i = 0; i < vfx.Length; i++)
        {
            if (vfx[i].name == trigger)
            {
                Debug.Log(vfx[i].name);
                vfx[i].Reinit();
                vfx[i].SendEvent("Trigger");
            }
        }        
    }

    public void StartSpecial()
    {
        soPlayer.soPlayerAttack.SpecialStart();
    }
}
