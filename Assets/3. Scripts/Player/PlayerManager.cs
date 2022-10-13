using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
    PlayerMap playerMap;
    bool movement;
    bool canDash = true;
    bool canAttack = true;
    bool dashing;
    bool canSpecial = true;
    bool canFire = true;
    public GameObject animated;
    public Animator animator;
    GameObject[] savePoints;
    int saveIndex;
    [HideInInspector]
    public float animAttack1Time;
    [HideInInspector]
    public float animAttack2Time;
    [HideInInspector]
    public float animAttack3Time;

    void Awake()
    {
        animator = animated.GetComponent<Animator>();

        playerMap = new PlayerMap();

        //Forma de chamar as funções sem precisar associar manualmente no inspector
        

    }
    
    void PlayerSetCommands()
    {
        /*
        playerMap.Default.Enable();
        playerMap.Default.Movement.started += MovementStarted;
        playerMap.Default.Movement.canceled += MovementCanceled;
        playerMap.Default.Dash.started += DashStarted;
        playerMap.Default.Attack.started += AttackStarted;
        playerMap.Default.Aim.started += AimStarted;
        playerMap.Default.Special.started += SpecialStarted;
        playerMap.Default.Suicide.started += SuicideStarted;
        */
    }

    void PlayerRemoveCommands()
    {
        /*
        playerMap.Default.Movement.started -= MovementStarted;
        playerMap.Default.Movement.canceled -= MovementCanceled;
        playerMap.Default.Dash.started -= DashStarted;
        playerMap.Default.Attack.started -= AttackStarted;
        playerMap.Default.Aim.started -= AimStarted;
        playerMap.Default.Special.started -= SpecialStarted;
        playerMap.Default.Suicide.started -= SuicideStarted;
        playerMap.Default.Disable();
        */
    }

    
    void SetConfiguration()
    {
        canDash = true;
        canAttack = true;
        dashing = false;
        canSpecial = true;
        canFire = true;

        soSave.savePoint = transform;
        soPlayer.soPlayerHealth.life = soPlayer.soPlayerHealth.maxLife;
        soPlayer.soPlayerMove.staminas = soPlayer.soPlayerMove.maxStaminas;
        soPlayer.soPlayerAttack.currentCooldown = soPlayer.soPlayerAttack.attackCooldown;
        soPlayer.soPlayerAttack.currentDamage = soPlayer.soPlayerAttack.attackDamage;
        soPlayer.soPlayerAttack.currentDuration = soPlayer.soPlayerAttack.attackDuration;
        soPlayer.soPlayerAttack.comboIndex = 0;
        soPlayer.soPlayerAttack.specialTime = 0;
        soPlayer.soPlayerMove.rechargeTime = 0;
        soPlayer.soPlayerHealth.fireCharges = 0;
        soPlayer.soPlayerMove.slowDuration = 0;
        soPlayer.soPlayerMove.vel = soPlayer.soPlayerMove.velBase;
        soPlayer.soPlayerHealth.burned = false;
        soPlayer.soPlayerMove.slow = false;
        soPlayer.soPlayerHealth.dead = false;
        soPlayer.canTeleport = false;
        soPlayer.soPlayerHealth.canDamaged = true;
        soPlayer.isPaused = false;
        soPlayer.soPlayerAttack.hitKill = false;
        soPlayer.soPlayerMove.superVelocity = false;


        soPlayer.state = SOPlayer.State.STOPPED;
    }
    
    private void Start() {
        if(SceneManager.GetActiveScene().name == "Level-00") soPlayer.SetLevel(0);
        else if(SceneManager.GetActiveScene().name == "Level-01") soPlayer.SetLevel(1);
        else if(SceneManager.GetActiveScene().name == "Level-02") soPlayer.SetLevel(2);
        else soPlayer.SetLevel(0);
        
        AnimationsTime();
        savePoints = GameObject.FindGameObjectsWithTag("SavePoint");

        SetConfiguration();
    }

    void Update()
    {
        //Debug.Log(soPlayer.state);
        if(movement) MovementPerformed(); //Forma pra que rode todo frame enquanto o botão estiver apertado

        if(soPlayer.soPlayerMove.staminas < soPlayer.soPlayerMove.maxStaminas) RechargeDash();
        if(!canSpecial) SpecialCooldown();
        if(soPlayer.soPlayerHealth.burned) Burn();
        if(soPlayer.soPlayerMove.slow) Slow();
    }

    bool IsDead()
    {
        return soPlayer.soPlayerHealth.dead || soPlayer.isPaused;
    }

    //-------------------------------MOVIMENTAÇÃO--------------------------------- 
    public void MovementStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                movement = true;
            }
            else if(context.canceled)
            {
                MovementCanceled(context);
            }
        }
            
        /*
        if(soPlayer.state == SOPlayer.State.STOPPED)
        {
            soPlayer.state = SOPlayer.State.WALKING;
            soPlayer.soPlayerMove.MoveStart();
            
        }
        else if(soPlayer.state == SOPlayer.State.DASHING)
        {
            soPlayer.soPlayerMove.MoveStart();
        }
        */
    }

    public void MovementPerformed()
    {
        if(!IsDead())
        {
            if(soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING)
            {
                //animator.SetBool("Move", true);
                animator.SetBool("Move", true);
                soPlayer.state = SOPlayer.State.WALKING;
                soPlayer.soPlayerMove.MoveStart();
                
            }
        }
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        movement = false;
        //animator.SetBool("Move", false);
        animator.SetBool("Move", false);
        if(!IsDead())
        {
            if(soPlayer.state == SOPlayer.State.WALKING)
            {
                soPlayer.state = SOPlayer.State.STOPPED;
                soPlayer.soPlayerMove.MoveEnd();
            }
        }
        /*
        else if(soPlayer.state == SOPlayer.State.DASHING)
        {
            soPlayer.soPlayerMove.MoveEnd();
        }
        */
        
    }
    //-------------------------------DASH--------------------------------- 
    public void DashStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                if(canDash && soPlayer.state != SOPlayer.State.TRAPPED && soPlayer.state != SOPlayer.State.SPECIAL && soPlayer.soPlayerMove.staminas > 0 && !soPlayer.soPlayerMove.slow)
                {
                    //animator.SetTrigger("Dash");
                    animator.SetTrigger("Dash");
                    soPlayer.soPlayerMove.ChangeStaminaCount(-1);
                    soPlayer.soPlayerMove.rechargeTime = 0;
                    dashing = true;
                    soPlayer.state = SOPlayer.State.DASHING;
                    canDash = false;
                    StartCoroutine(DashCooldown());
                    soPlayer.soPlayerMove.DashStart();

                }
                else if(soPlayer.state == SOPlayer.State.TRAPPED)
                {
                    soPlayer.soPlayerMove.TrappedClick();
                }
            }
        }
    }

    void RechargeDash()
    {
        soPlayer.soPlayerMove.rechargeTime += Time.deltaTime;
            if(soPlayer.soPlayerMove.rechargeTime >= soPlayer.soPlayerMove.rechargeStaminasTime)
            { 
                soPlayer.soPlayerMove.RechargeStamina();
                soPlayer.soPlayerMove.rechargeTime = 0;
            }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerMove.dashDuration);
        dashing = false;
        yield return new WaitForSeconds(soPlayer.soPlayerMove.dashCooldown);
        canDash = true;
    }
    //-------------------------------ATAQUE--------------------------------- 
    public void AttackStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                if(!dashing && (soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING) && canAttack)
                {
                    //animator.SetTrigger("Ataque");
                    animator.SetInteger("AtaqIndex", soPlayer.soPlayerAttack.comboIndex);
                    animator.SetTrigger("Ataque");
                    canAttack = false;
                    soPlayer.state = SOPlayer.State.ATTACKING;
                    soPlayer.soPlayerAttack.AttackStart();
                    StartCoroutine(AttackCooldown());
                }
            }
        }
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerAttack.currentCooldown + soPlayer.soPlayerAttack.currentDuration);
        if(soPlayer.soPlayerAttack.currentCooldown == soPlayer.soPlayerAttack.damagedCooldown) soPlayer.soPlayerAttack.currentCooldown = soPlayer.soPlayerAttack.attackCooldown;
        canAttack = true;
    }

    void DamagedCooldown()
    {
        StopAllCoroutines();
        canAttack = false;
        canDash = true;
        dashing = false;
        soPlayer.soPlayerAttack.currentCooldown = soPlayer.soPlayerAttack.damagedCooldown;
        StartCoroutine(AttackCooldown());
    }

    //-------------------------------------------MIRAR-----------------------------------------------
    public void AimStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                if(soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING)
                {
                    soPlayer.soPlayerMove.AimStart();
                }
            }
        }
    }

    //---------------------------------------------ESPECIAL------------------------------------------

    public void SpecialStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                if(!dashing && (soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING) && canSpecial)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Especial", transform.position);
                    soPlayer.soPlayerAttack.SpecialStart();
                    soPlayer.state = SOPlayer.State.SPECIAL;
                    soPlayer.soPlayerMove.DashStart();
                    canSpecial = false;
                }
            }
        }
    }
    void SpecialCooldown()
        {
            soPlayer.soPlayerAttack.specialTime += Time.deltaTime;
            if(soPlayer.soPlayerAttack.specialTime >= soPlayer.soPlayerAttack.specialCooldown)
            {
                soPlayer.soPlayerAttack.specialTime = 0;
                canSpecial = true;
            }
        }

    //----------------------------------------------RESTART------------------------------------------

    void Restart()
    {
        soPlayer.soPlayerHealth.dead = false;
        GetComponent<CharacterController>().enabled = false;
        transform.position = soSave.savePoint.position;
        GetComponent<CharacterController>().enabled = true;
        SetConfiguration();
    }

    //--------------------------------------------FOGO------------------------------------------

    void Burn()
    {
            if(soPlayer.soPlayerHealth.fireCharges > 0)
            {
                if(canFire)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Dano_Fogo", transform.position);
                    canFire = false;
                    StartCoroutine(CooldownFireDamage());
                    soPlayer.soPlayerHealth.HealthChange(-soPlayer.soPlayerHealth.fireDamage);
                }
                else
                {
                    soPlayer.soPlayerHealth.fireCharges -= Time.deltaTime;
                }
                
            }
            else
            {
                canFire = true;
                soPlayer.soPlayerHealth.burned = false;
                soPlayer.soPlayerHealth.HealthChange(-soPlayer.soPlayerHealth.fireDamage);
            }
    }

    IEnumerator CooldownFireDamage()
    {
        yield return new WaitForSeconds(soPlayer.soPlayerHealth.flameDelay);
        canFire = true;
    }

    //---------------------------------------------LENTIDÃO------------------------------------------

    void Slow()
    {
        soPlayer.soPlayerMove.slowDuration -= Time.deltaTime;
        if(soPlayer.soPlayerMove.slowDuration <= 0)
        {
            soPlayer.soPlayerMove.vel = soPlayer.soPlayerMove.velBase;
            soPlayer.soPlayerMove.slow = false;
        }
    }
    //------------------------------------------DANIFICADO-----------------------------------------

    void Damaged()
    {
        //animator.SetTrigger("Dano");
        animator.SetTrigger("Dano");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Dano_Humano", transform.position);
    }

    //--------------------------------------------SE MATAR----------------------------------------

    public void SuicideStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                soPlayer.soPlayerHealth.HealthChange(-100);
            }
        }
    }

    //-------------------------------------------MORRER-----------------------------------------------

    public void OnDie() 
    {
        StartCoroutine(TimeRestart());
    }

    IEnumerator TimeRestart()
    {
        yield return new WaitForSeconds(10);
        soSave.Restart();
    }
    
    //--------------------------------------------PAUSE-----------------------------------------------

    public void PauseStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(context.started)
            {
                soPlayer.Pause();
            }
        }
            
    }

    //------------------------------------------TELEPORTE---------------------------------------------

    public void TeleportStarted(InputAction.CallbackContext context)
    {
        if(!IsDead())
        {
            if(soPlayer.canTeleport)
            {
                if(context.started)
                {
                    GetComponent<CharacterController>().enabled = false;
                    transform.position = savePoints[saveIndex].transform.position;
                    GetComponent<CharacterController>().enabled = true;
                    saveIndex++;
                    if(saveIndex >= savePoints.Length) saveIndex = 0;
                }
            }
        }
    }

    //------------------------------------------TEMPO DE ANIMAÇÕES-----------------------------------

    void AnimationsTime()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Ataque1_Caipora":
                    animAttack1Time = clip.length;
                    break;
                case "Ataque2_Caipora":
                    animAttack2Time = clip.length;
                    break;
                case "Ataque3_Caipora":
                    animAttack3Time = clip.length;
                    break;
            }
        }
    }

    //-------------------------------------------LISTENER---------------------------------------------
    public void OnEnable()
    {
        PlayerSetCommands();
        //soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(DamagedCooldown);
        soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(Damaged);
        soSave.RestartEvent.AddListener(Restart);
        soPlayer.LevelUpEvent.AddListener(SetConfiguration);
        soPlayer.soPlayerHealth.DieEvent.AddListener(OnDie);
    }
    public void OnDisable()
    {
        //soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(DamagedCooldown);
        soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(Damaged);
        soSave.RestartEvent.RemoveListener(Restart);
        soPlayer.LevelUpEvent.RemoveListener(SetConfiguration);
        soPlayer.soPlayerHealth.DieEvent.RemoveListener(OnDie);
    }
}
