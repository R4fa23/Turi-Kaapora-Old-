using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public SOPlayer soPlayer;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    public Material attack;
    bool trapped;
    bool special;
    public PlayerManager playerManager;
    float attackTime;
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
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
        soPlayer.soPlayerAttack.comboIndex++;
        if (soPlayer.soPlayerAttack.comboIndex == 1)
        {
            attack.color = Color.white;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Ataque_Leve", transform.position);
            attackTime = playerManager.animAttack1Time;
            soPlayer.soPlayerAttack.currentDuration = playerManager.animAttack1Time;
        }
        else if (soPlayer.soPlayerAttack.comboIndex == 2)
        {
            attack.color = Color.yellow;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Ataque_Leve_2", transform.position);
            attackTime = playerManager.animAttack2Time;
            soPlayer.soPlayerAttack.currentDuration = playerManager.animAttack2Time;
        }
        else
        {
            attack.color = Color.red;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Ataque_Leve_3", transform.position);
            attackTime = playerManager.animAttack3Time;
            soPlayer.soPlayerAttack.currentDuration = playerManager.animAttack3Time;
        }

        StartCoroutine(AttackTime(attackTime));
    }

    public void AttackEnd()
    {
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        if(!trapped && !special)soPlayer.state = SOPlayer.State.STOPPED;
        if(soPlayer.soPlayerAttack.comboIndex >= 3) ComboEnd();
    }

    public void ComboEnd()
    {
        StopAllCoroutines();
        soPlayer.soPlayerAttack.comboIndex = 0;
        soPlayer.soPlayerAttack.currentCooldown = soPlayer.soPlayerAttack.attackCooldown;
        soPlayer.soPlayerAttack.currentDuration = soPlayer.soPlayerAttack.attackDuration;
        soPlayer.soPlayerAttack.currentDamage = soPlayer.soPlayerAttack.attackDamage;
    }

    void Trapped()
    {
        trapped = true;
    }
    void Untrapped()
    {
        trapped = false;
    }

    void SpecialStart()
    {
        special = true;
    }

    void SpecialFinish()
    {
        special = false;
    }

    public void OnEnable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.AddListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(ComboEnd);
        //soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(AttackEnd);
        //soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(ComboEnd);
        soPlayer.soPlayerAttack.SpecialStartEvent.AddListener(SpecialStart);
        soPlayer.soPlayerAttack.SpecialFinishEvent.AddListener(SpecialFinish);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerAttack.AttackStartEvent.RemoveListener(AttackStart);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(AttackEnd);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(ComboEnd);
        //soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(AttackEnd);
        //soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(ComboEnd);
        soPlayer.soPlayerAttack.SpecialStartEvent.RemoveListener(SpecialStart);
        soPlayer.soPlayerAttack.SpecialFinishEvent.RemoveListener(SpecialFinish);
    }

    IEnumerator AttackTime(float time)
    {
        yield return new WaitForSeconds(time);
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
            if (soPlayer.soPlayerAttack.comboIndex == 3)
            {
                soPlayer.soPlayerAttack.ReduceCooldown();
            }

            if(!soPlayer.soPlayerAttack.hitKill)other.transform.parent.transform.parent.GetComponent<EnemyManager>().soEnemy.ChangeLife(-soPlayer.soPlayerAttack.currentDamage);
            else other.transform.parent.transform.parent.GetComponent<EnemyManager>().soEnemy.ChangeLife(-1000);
            
        }

        if(other.CompareTag("Cage"))
        {
            other.GetComponent<Cage>().LoseLife();
        }
    }
}
