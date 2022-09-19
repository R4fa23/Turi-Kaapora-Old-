using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    Animator animator;

    void Awake()
    {
        animator = animated.GetComponent<Animator>();

        playerMap = new PlayerMap();

        //Forma de chamar as funções sem precisar associar manualmente no inspector
        playerMap.Default.Enable();
        playerMap.Default.Movement.started += MovementStarted;
        playerMap.Default.Movement.canceled += MovementCanceled;
        playerMap.Default.Dash.started += DashStarted;
        playerMap.Default.Attack.started += AttackStarted;
        playerMap.Default.Aim.started += AimStarted;
        playerMap.Default.Special.started += SpecialStarted;

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
        soPlayer.state = SOPlayer.State.STOPPED;
    }
    
    private void Start() {
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

    //-------------------------------MOVIMENTAÇÃO--------------------------------- 
    public void MovementStarted(InputAction.CallbackContext context)
    {
        movement = true;
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
        if(soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING)
        {
            animator.SetBool("Move", true);
            soPlayer.state = SOPlayer.State.WALKING;
            soPlayer.soPlayerMove.MoveStart();
            
        }
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        movement = false;
        animator.SetBool("Move", false);

        if(soPlayer.state == SOPlayer.State.WALKING)
        {
            soPlayer.state = SOPlayer.State.STOPPED;
            soPlayer.soPlayerMove.MoveEnd();
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
        if(canDash && soPlayer.state != SOPlayer.State.TRAPPED && soPlayer.state != SOPlayer.State.SPECIAL && soPlayer.soPlayerMove.staminas > 0 && !soPlayer.soPlayerMove.slow)
        {
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
        if(!dashing && (soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING) && canAttack)
        {
            animator.SetTrigger("Ataque");
            canAttack = false;
            soPlayer.state = SOPlayer.State.ATTACKING;
            soPlayer.soPlayerAttack.AttackStart();
            StartCoroutine(AttackCooldown());
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
        if(soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING)
        {
            soPlayer.soPlayerMove.AimStart();
        }
            
    }

    //---------------------------------------------ESPECIAL------------------------------------------

    public void SpecialStarted(InputAction.CallbackContext context)
    {
        if(!dashing && (soPlayer.state == SOPlayer.State.STOPPED || soPlayer.state == SOPlayer.State.WALKING) && canSpecial)
        {
            soPlayer.soPlayerAttack.SpecialStart();
            soPlayer.state = SOPlayer.State.SPECIAL;
            soPlayer.soPlayerMove.DashStart();
            canSpecial = false;
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
        
        GetComponent<CharacterController>().enabled = false;
        transform.position = soSave.savePoint.position;
        GetComponent<CharacterController>().enabled = true;
        SetConfiguration();
    }

    //--------------------------------------------QUEIMANDO------------------------------------------

    void Burn()
    {
            if(soPlayer.soPlayerHealth.fireCharges > 0)
            {
                if(canFire)
                {
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
        animator.SetTrigger("Dano");
    }
    //-------------------------------------------LISTENER---------------------------------------------
    public void OnEnable()
    {
        //soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(DamagedCooldown);
        soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(Damaged);
        soSave.RestartEvent.AddListener(Restart);
        soPlayer.LevelUpEvent.AddListener(SetConfiguration);
    }
    public void OnDisable()
    {
        //soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(DamagedCooldown);
        soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(Damaged);
        soSave.RestartEvent.RemoveListener(Restart);
        soPlayer.LevelUpEvent.RemoveListener(SetConfiguration);
    }
}
