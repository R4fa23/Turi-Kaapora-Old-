using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class TriggerVFX : MonoBehaviour
{
    public SOPlayer soPlayer;
    public VisualEffect[] attacks;
    public VisualEffect[] enemy;
    public VisualEffect special;
    public VisualEffect dash;
    public Transform player;
    //public string eventToTrigger;
      
    public void Attack(int vfxIndex)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            if (i == vfxIndex)
            {
                attacks[i].SetFloat("Y Angle", player.eulerAngles.y);
                attacks[i].Reinit();
                attacks[i].SendEvent("Attack");
                //Debug.Log(i);
            }
        }        
    }

    public void Enemy()
    {
        foreach (var item in enemy)
        {
            item.Reinit();
            item.SendEvent("Attack");
        }
    }

    public void SparksAttack2(int start)
    {
        for (int i = 0; i < attacks.Length; i++)
        {
            if (i == 1)
            {
                if (start == 1) attacks[i].SendEvent("start sparks");
                else attacks[i].SendEvent("stop sparks");
            }
        }
        
    }
    public void Special()
    {
        special.Reinit();
        special.SendEvent("Special");
    }

    public void StartDash()
    {
        dash.SendEvent("StartDash");
        dash.SetBool("Trail Dash", true);
        StartCoroutine(DashCounter());
    }

    IEnumerator DashCounter()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerMove.dashDuration);
        dash.SendEvent("EndDash");
        dash.SetBool("Trail Dash", false);
    }

    public void StartSpecial()
    {
        soPlayer.soPlayerAttack.SpecialStart();
    }
}
