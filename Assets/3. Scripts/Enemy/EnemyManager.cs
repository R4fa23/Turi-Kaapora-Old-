using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public SOEnemy reference;
    //[HideInInspector]
    public SOEnemy soEnemy;
    public SOSave soSave;
    public SOPlayer soPlayer;
    GameObject player;
    bool repulsionCooldown;
    public Animator animator;
    float animChargeTime;
    float animAttackTime;

    void Awake()
    {
        soEnemy = (SOEnemy)ScriptableObject.CreateInstance(typeof(SOEnemy));
        AnimationsTime();
        SetConfiguration();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Repulse()
    {
        if(!repulsionCooldown)
        {
            Vector3 dirView = player.transform.position - transform.position;
            dirView.y = 0;
            transform.forward = dirView;

            if(soEnemy.canDamaged) StartCoroutine(Repulsion(soEnemy.forceRecover));
            else StartCoroutine(Repulsion(soPlayer.soPlayerAttack.repulsionSpecialForce));
        }
    }

    IEnumerator Repulsion(float force)
    {
        repulsionCooldown = true;
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(-transform.forward * force, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = true;
        StartCoroutine(RepulsionCooldown());
    }

    IEnumerator RepulsionCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        repulsionCooldown = false;
    }

    void RecoverDamage()
    {
        StartCoroutine(TimeRecoverDamage());
    }

    void Restart()
    {
        animator.SetTrigger("Restart");
    }

    void StartCharge()
    {
        animator.SetTrigger("Start Charge");
    }

    void Damaged()
    {
        animator.SetTrigger("Take Damage");
    }

    IEnumerator TimeRecoverDamage()
    {
        yield return new WaitForSeconds(0.1f);
        soEnemy.canDamaged = true;
    }

    public void OnEnable()
    {
        soEnemy.canDamaged = true;
        soEnemy.health = soEnemy.maxHealth;

        soEnemy.RepulsionEvent.AddListener(RecoverDamage);
        if(soEnemy.enemyType != SOEnemy.EnemyType.LUMBERJACK) soEnemy.AttackEndEvent.AddListener(Repulse);
        soEnemy.DieEvent.AddListener(OnDie);
        soEnemy.RepulsionEvent.AddListener(Repulse);
        soSave.RestartEvent.AddListener(Restart);
        soEnemy.ChargeStartEvent.AddListener(StartCharge);
        soEnemy.ChangeLifeEvent.AddListener(Damaged);
    }
    public void OnDisable()
    {
        soEnemy.RepulsionEvent.RemoveListener(RecoverDamage);
        if(soEnemy.enemyType != SOEnemy.EnemyType.LUMBERJACK) soEnemy.AttackEndEvent.RemoveListener(Repulse);
        soEnemy.DieEvent.RemoveListener(OnDie);
        soEnemy.RepulsionEvent.RemoveListener(Repulse);
        soSave.RestartEvent.RemoveListener(Restart);
        soEnemy.ChargeStartEvent.RemoveListener(StartCharge);
        soEnemy.ChangeLifeEvent.RemoveListener(Damaged);
    }

    private void OnDie() 
    {
        soPlayer.soPlayerAttack.EnemyDie(this.gameObject);
    }

    void AnimationsTime()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Charge Attack":
                    animChargeTime = clip.length;
                    break;
                case "Attack":
                    animAttackTime = clip.length;
                    break;
            }
        }
    }

    void SetConfiguration()
    {   

        soEnemy.enemyType = reference.enemyType;
        soEnemy.attackDamage = reference.attackDamage;
        soEnemy.attackChargeDuration = animChargeTime;
        soEnemy.attackDuration = animAttackTime;
        soEnemy.attackCooldown = reference.attackCooldown;
        soEnemy.attackRange = reference.attackRange;
        soEnemy.vel = reference.vel;
        soEnemy.distanceDetectation = reference.distanceDetectation;
        soEnemy.maxHealth = reference.maxHealth;
        soEnemy.health = reference.health;
        soEnemy.cooldownDamaged = reference.cooldownDamaged;
        soEnemy.currentCooldown = reference.currentCooldown;
        soEnemy.canDamaged = true;
        soEnemy.divisorAttackChance = reference.divisorAttackChance;
        soEnemy.maxSecondsToAttack = reference.maxSecondsToAttack;
        soEnemy.forceRecover = reference.forceRecover;
        soEnemy.specialTime = 0;
        soEnemy.timeToSpecial = reference.timeToSpecial;
        soEnemy.canAttack = true;
        soEnemy.attackTime = 0;
        soEnemy.timeToAttack = reference.timeToAttack;




        soEnemy.currentCooldown = soEnemy.attackCooldown;
        soEnemy.health = soEnemy.maxHealth;
    }
}
