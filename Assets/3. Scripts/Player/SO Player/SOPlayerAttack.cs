using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerAttack : ScriptableObject
{
    public float attackDuration;
    public float attackCooldown;
    public float attackComboFinalDuration;
    public float attackComboFinalCooldown;
    public int attackDamage;
    public float comboTime;
    public int comboDamage;
    public float damagedCooldown;
    [HideInInspector]
    public float currentCooldown;
    [HideInInspector]
    public float currentDuration;
    [HideInInspector]
    public int currentDamage;
    [HideInInspector]
    public int comboIndex = 0;
    public int specialDamage;
    public float specialDuration;
    public float specialCooldown;
    [HideInInspector]
    public float specialTime;
    public float cooldownReduction;


    [System.NonSerialized]
    public UnityEvent AttackStartEvent;
    [System.NonSerialized]
    public UnityEvent<GameObject> EnemyDieEvent;
    [System.NonSerialized]
    public UnityEvent SpecialStartEvent;
    [System.NonSerialized]
    public UnityEvent SpecialFinishEvent;

    private void OnEnable() {
        if(AttackStartEvent == null)
            AttackStartEvent = new UnityEvent();

        if(EnemyDieEvent == null)
            EnemyDieEvent = new UnityEvent<GameObject>();

        if(SpecialStartEvent == null)
            SpecialStartEvent = new UnityEvent();
        
        if(SpecialFinishEvent == null)
            SpecialFinishEvent = new UnityEvent();
    }

    public void AttackStart()
    {
        if(comboIndex == 2)
        {
            currentDamage = comboDamage;
            currentCooldown = attackComboFinalCooldown;
            currentDuration = attackComboFinalDuration;
        }
        else{
            currentDamage = attackDamage;
            currentCooldown = attackCooldown;
            currentDuration = attackDuration;
        }
        AttackStartEvent.Invoke();
    }

    public void EnemyDie(GameObject enemy)
    {
        EnemyDieEvent.Invoke(enemy);
    }

    public void SpecialStart()
    {
        SpecialStartEvent.Invoke();
    }

    public void SpecialFinish()
    {
        SpecialFinishEvent.Invoke();
    }

    public void ReduceCooldown()
    {
        specialTime += cooldownReduction;
    }
}
