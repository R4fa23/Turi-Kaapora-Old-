using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOFort soFort;

    public void Invincibility(bool enabled)
    {
        soPlayer.soPlayerHealth.canDamaged = !enabled;
    }

    public void LevelSlider(Single level)
    {
        soPlayer.SetLevel((int)level - 1);
    }

    public void HitKill(bool enabled)
    {
        soPlayer.soPlayerAttack.hitKill = enabled;
    }

    public void CompleteAllChallenges()
    {
        soFort.CompleteChallenges();
    }

    public void CompleteTheFort()
    {
        soFort.CompleteFort();
    }

    public void CanTeleport(bool enabled)
    {
        soPlayer.canTeleport = enabled;
    }


}
