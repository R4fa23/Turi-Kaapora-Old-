using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [HideInInspector]
    public SOTrail soTrail;
    public SOPlayer soPlayer;
    int cageLife = 3;
    
    public void LoseLife()
    {
        cageLife--;
        if(cageLife <= 0) Break();
    }

    void Break()
    {
        soPlayer.soPlayerAttack.EnemyDie(this.gameObject);
        soTrail.BreakCage();
        this.gameObject.SetActive(false);
    }
}
