using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TriggerVFX : MonoBehaviour
{
    public SOPlayer soPlayer;
    public VisualEffect vfx;
    public string eventToTrigger;
    public void VFXEvent(string trigger)
    {
        vfx.Reinit();
        vfx.SendEvent(eventToTrigger);
    }

    public void StartSpecial()
    {
        soPlayer.soPlayerAttack.SpecialStart();
    }
}
