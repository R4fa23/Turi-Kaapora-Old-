using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    
    void Start()
    {
        soPlayer.soPlayerAttack.comboIndex = 0;
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void AttackStart()
    {
        StopAllCoroutines();
        StartCoroutine(ComboTime());
        StartCoroutine(AttackTime());
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
        soPlayer.soPlayerAttack.comboIndex++;
    }

    public void AttackEnd()
    {
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        soPlayer.state = SOPlayer.State.STOPPED;
        if(soPlayer.soPlayerAttack.comboIndex >= 3) ComboEnd();
    }

    public void ComboEnd()
    {
        StopAllCoroutines();
        soPlayer.soPlayerAttack.comboIndex = 0;
    }

    public void OnEnable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.AddListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(ComboEnd);
        soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(AttackEnd);
        soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(ComboEnd);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.RemoveListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(ComboEnd);
        soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(AttackEnd);
        soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(ComboEnd);
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

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        { 
            other.GetComponent<EnemyManager>().soEnemy.ChangeLife(-soPlayer.soPlayerAttack.currentDamage);
        }
    }
}
