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
    [HideInInspector]
    public float animWaitTime;
    [HideInInspector]
    public float animChargeEspTime;
    [HideInInspector]
    public float animAttackEspTime;

    void Awake()
    {
        soEnemy = (SOEnemy)ScriptableObject.CreateInstance(typeof(SOEnemy));
        soEnemy.enemyType = reference.enemyType;
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

    void StartEspCharge()
    {
        animator.SetTrigger("Start Esp Charge");
    }

    void Damaged()
    {
        animator.SetTrigger("Take Damage");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Acerto", transform.position);
    }

    IEnumerator TimeRecoverDamage()
    {
        yield return new WaitForSeconds(0.1f);
        soEnemy.canDamaged = true;
    }

    void Summoned()
    {
        Vector3 dirView = player.transform.position - transform.position;
        dirView.y = 0;
        transform.forward = dirView;
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
        soEnemy.ChargeEspStartEvent.AddListener(StartEspCharge);
        soEnemy.ChangeLifeEvent.AddListener(Damaged);
        soEnemy.SummonEvent.AddListener(Summoned);
    }
    public void OnDisable()
    {
        soEnemy.RepulsionEvent.RemoveListener(RecoverDamage);
        if(soEnemy.enemyType != SOEnemy.EnemyType.LUMBERJACK) soEnemy.AttackEndEvent.RemoveListener(Repulse);
        soEnemy.DieEvent.RemoveListener(OnDie);
        soEnemy.RepulsionEvent.RemoveListener(Repulse);
        soSave.RestartEvent.RemoveListener(Restart);
        soEnemy.ChargeStartEvent.RemoveListener(StartCharge);
        soEnemy.ChargeEspStartEvent.RemoveListener(StartEspCharge);
        soEnemy.ChangeLifeEvent.RemoveListener(Damaged);
        soEnemy.SummonEvent.RemoveListener(Summoned);
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
            if(soEnemy.enemyType == SOEnemy.EnemyType.MELEE)
            {
                switch(clip.name)
                {
                    case "Enemy_peao_ChargeAtq":
                        animChargeTime = clip.length;
                        break;
                    case "Enemy_peao_Atq":
                        animAttackTime = clip.length;
                        break;
                    case "Enemy_peao_Wait":
                        animWaitTime = clip.length;
                        break;
                }
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.RANGE)
            {
                switch(clip.name)
                {
                    case "Enemy_Atirador_ChargeAtq":
                        animChargeTime = clip.length;
                        break;
                    case "Enemy_Atirador_Atq":
                        animAttackTime = clip.length;
                        break;
                    case "Enemy_Atirador_Wait":
                        animWaitTime = clip.length;
                        break;
                }
                
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.INCENDIARY)
            {
                switch(clip.name)
                {
                    case "Enemy_Incen_ChageAtq":
                        animChargeTime = clip.length;
                        break;
                    case "Enemy_Incen_Atq":
                        animAttackTime = clip.length;
                        break;
                    case "Enemy_Incen_Wait":
                        animWaitTime = clip.length;
                        break;
                }
                
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.HUNTER)
            {
                switch(clip.name)
                {
                    case "Enemy_cacador_ChargeAtq":
                        animChargeTime = clip.length;
                        break;
                    case "Enemy_cacador_Atq":
                        animAttackTime = clip.length;
                        break;
                    case "Enemy_cacador_Wait":
                        animWaitTime = clip.length;
                        break;
                }
                
            }
            else if(soEnemy.enemyType == SOEnemy.EnemyType.LUMBERJACK)
            {
                switch(clip.name)
                {
                    case "Enemy_lenhador_ChageAtq":
                        animChargeTime = clip.length;
                        break;
                    case "Enemy_lenhador_Atq":
                        animAttackTime = clip.length;
                        break;
                    case "Enemy_lenhador_Wait":
                        animWaitTime = clip.length;
                        break;
                    case "Enemy_lenhador_ChargeEspAtq":
                        animChargeEspTime = clip.length;
                        break;
                    case "Enemy_lenhador_EspAtq":
                        animAttackEspTime = clip.length;
                        break;
                }
                
            }
        }
    }

    void SetConfiguration()
    {   

        soEnemy.enemyType = reference.enemyType;
        soEnemy.attackDamage = reference.attackDamage;
        soEnemy.attackChargeDuration = animChargeTime;
        soEnemy.attackDuration = animAttackTime;
        soEnemy.attackWaitDuration = animWaitTime;
        soEnemy.attackRange = reference.attackRange;
        soEnemy.vel = reference.vel;
        soEnemy.distanceDetectation = reference.distanceDetectation;
        soEnemy.maxHealth = reference.maxHealth;
        soEnemy.health = reference.health;
        soEnemy.canDamaged = true;
        soEnemy.divisorAttackChance = reference.divisorAttackChance;
        soEnemy.maxSecondsToAttack = reference.maxSecondsToAttack;
        soEnemy.forceRecover = reference.forceRecover;
        soEnemy.specialTime = 0;
        soEnemy.timeToSpecial = reference.timeToSpecial;
        soEnemy.canAttack = false;
        soEnemy.attackTime = 0;
        soEnemy.timeToAttack = reference.timeToAttack;
        soEnemy.rotationVel = reference.rotationVel;
        soEnemy.minTimeToRandomize = reference.minTimeToRandomize;
        soEnemy.attacked = false;
        soEnemy.timeToAttackAfterAttacked = reference.timeToAttackAfterAttacked;




        soEnemy.health = soEnemy.maxHealth;
    }
}
