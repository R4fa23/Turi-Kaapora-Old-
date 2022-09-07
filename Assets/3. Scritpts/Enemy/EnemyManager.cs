using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SOEnemy reference;
    [HideInInspector]
    public SOEnemy soEnemy;
    public SOPlayer soPlayer;

    void Awake()
    {
        soEnemy = (SOEnemy)ScriptableObject.CreateInstance(typeof(SOEnemy));
        SetConfiguration();

    }

    public void OnEnable()
    {
        soEnemy.DieEvent.AddListener(OnDie);
    }
    public void OnDisable()
    {
        soEnemy.DieEvent.RemoveListener(OnDie);
    }

    private void OnDie() 
    {
        soPlayer.soPlayerAttack.EnemyDie(this.gameObject);
        gameObject.SetActive(false);
    }

    void SetConfiguration()
    {
        soEnemy.attackDamage = reference.attackDamage;
        soEnemy.attackChargeDuration = reference.attackChargeDuration;
        soEnemy.attackDuration = reference.attackDuration;
        soEnemy.attackCooldown = reference.attackCooldown;
        soEnemy.attackRange = reference.attackRange;
        soEnemy.vel = reference.vel;
        soEnemy.distanceDetectation = reference.distanceDetectation;
        soEnemy.maxHealth = reference.maxHealth;
        soEnemy.health = reference.health;
        soEnemy.cooldownDamaged = reference.cooldownDamaged;
        soEnemy.currentCooldown = reference.currentCooldown;


        soEnemy.currentCooldown = soEnemy.attackCooldown;
        soEnemy.health = soEnemy.maxHealth;
    }
}
