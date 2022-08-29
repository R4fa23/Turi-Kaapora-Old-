using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    BoxCollider boxCollider;
    
    void Start()
    {
        soPlayer.soPlayerAttack.comboIndex = 0;
        boxCollider = GetComponent<BoxCollider>();
    }
    public void AttackStart()
    {
        StopAllCoroutines();
        StartCoroutine(ComboTime());
        StartCoroutine(AttackTime());
        boxCollider.enabled = true;
        soPlayer.soPlayerAttack.comboIndex++;
    }

    public void AttackEnd()
    {
        boxCollider.enabled = false;
        soPlayer.state = SOPlayer.State.STOPPED;
        if(soPlayer.soPlayerAttack.comboIndex >= 3) ComboEnd();
    }

    public void ComboEnd()
    {
        soPlayer.soPlayerAttack.comboIndex = 0;
    }

    public void OnEnable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.AddListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(ComboEnd);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.RemoveListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(ComboEnd);
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerAttack.currentDuration);
        AttackEnd();
    }

    IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerAttack.comboTime);
        ComboEnd();
    }
}
