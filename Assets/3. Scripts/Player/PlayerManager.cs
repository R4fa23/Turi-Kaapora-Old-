using System.Dynamic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PlayerManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
    PlayerMap playerMap;
    bool movement;
    bool canDash = true;
    bool canAttack = true;
    bool dashing;
    [HideInInspector] public bool canSpecial = true;
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
    float currentStamina;
    float lastStamina;
    PlayerInput pInput;
    InputAction moving;


    [ColorUsage(true, true)]
    public Color redColor;
    [ColorUsage(true, true)]
    public Color blueColor;
    [ColorUsage(true, true)]
    public Color whiteColor;
    [ColorUsage(true, true)]
    public Color nextColor;
    [SerializeField] SkinnedMeshRenderer caiporaBody;
    [SerializeField] VisualEffect fire;
    [Range(0,1)]
    public float speedFeedbackDamage;
    void Awake()
    {
        pInput = GetComponent<PlayerInput>();
        moving = pInput.actions["Movement"];

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
        soPlayer.soPlayerAttack.specialTime = soPlayer.soPlayerAttack.specialCooldown;
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
        soPlayer.isCutscene = false;
        soPlayer.soPlayerAttack.hitKill = false;
        soPlayer.soPlayerMove.superVelocity = false;


        soPlayer.state = SOPlayer.State.STOPPED;
    }

    private void Start() {
        if (SceneManager.GetActiveScene().name == "Level-00") soPlayer.SetLevel(0);
        else if (SceneManager.GetActiveScene().name == "Level-01") soPlayer.SetLevel(1);
        else if (SceneManager.GetActiveScene().name == "Level-02") soPlayer.SetLevel(2);
        else soPlayer.SetLevel(0);

        AnimationsTime();
        savePoints = GameObject.FindGameObjectsWithTag("SavePoint");

        SetConfiguration();
    }

    void Update()
    {
        //Debug.Log(soPlayer.state);
        if (movement) MovementPerformed(); //Forma pra que rode todo frame enquanto o botão estiver apertado

        if (soPlayer.soPlayerMove.staminas < soPlayer.soPlayerMove.maxStaminas) RechargeDash();
        if (!canSpecial) SpecialCooldown();
        if (soPlayer.soPlayerHealth.burned) Burn();
        if (soPlayer.soPlayerMove.slow) Slow();

        currentStamina = soPlayer.soPlayerMove.maxStaminas;

        if (currentStamina != lastStamina)
        {
            lastStamina = currentStamina;
            soPlayer.soPlayerMove.ChangeMaxStamina();
        }
        DamageFeedback();         
    }

    public void DamageFeedback()
    {
        if (nextColor != caiporaBody.material.GetColor("_Color"))
        {
            caiporaBody.material.SetColor("_Color", Vector4.Lerp(caiporaBody.material.GetColor("_Color"), nextColor, speedFeedbackDamage));
        }

        if (caiporaBody.material.GetColor("_Color") == redColor || caiporaBody.material.GetColor("_Color") == blueColor)
        {
            nextColor = whiteColor;
        }
    }
    bool IsDead()
    {
        return soPlayer.soPlayerHealth.dead || soPlayer.isPaused || soPlayer.isCutscene;
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
                animator.SetBool("Move", true);
                soPlayer.state = SOPlayer.State.WALKING;
                soPlayer.soPlayerMove.MoveStart();
                
            }
        }
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        movement = false;
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
                    animator.SetTrigger("Especial");
                    //soPlayer.soPlayerAttack.SpecialStart();
                    soPlayer.state = SOPlayer.State.SPECIAL;
                    //soPlayer.soPlayerMove.DashStart();
                    soPlayer.soPlayerAttack.specialTime = 0;
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
                soPlayer.soPlayerAttack.specialTime = soPlayer.soPlayerAttack.specialCooldown;
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
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Vozes/Dano", transform.position);
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
        //fire.Reinit();
        fire.SendEvent("StartFire");
        yield return new WaitForSeconds(soPlayer.soPlayerHealth.flameDelay);
        canFire = true;
        //fire.SendEvent("StopFire");
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
        if (soPlayer.state != SOPlayer.State.SPECIAL)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Dano_Humano", transform.position);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Vozes/Dano", transform.position);
            animator.SetTrigger("Dano");
            nextColor = redColor;
        }
    }
    
    void Heal()
    {
        if (soPlayer.state != SOPlayer.State.SPECIAL)
        {   //Healing
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Dano_Humano", transform.position);
            //FMODUnity.RuntimeManager.PlayOneShot("event:/Vozes/Dano", transform.position);
            nextColor = blueColor;
        }
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
        animator.SetTrigger("Morte");
        FMODUnity.RuntimeManager.PlayOneShot("event:/Vozes/Morte", transform.position);
        StartCoroutine(TimeRestart());
    }

    IEnumerator TimeRestart()
    {
        yield return new WaitForSeconds(2);
        animator.SetTrigger("Revive");
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

    //-------------------------------------------CUTSCENE---------------------------------------------

    public void StartCutscene()
    {
        soPlayer.StartCutscene();
    }
    public void EndCutscene()
    {
        soPlayer.EndCutscene();
    }

    public void InCutscene()
    {
        animator.SetBool("Move", false);
        animator.SetTrigger("Idle");
    }

    public void OutCutscene()
    {
        if (moving.ReadValue<Vector2>().magnitude <= 0.1f) movement = false;
    }

    //-------------------------------------------LISTENER---------------------------------------------
    public void OnEnable()
    {
        PlayerSetCommands();
        //soPlayer.soPlayerHealth.DamageHealthChangeEvent.AddListener(DamagedCooldown);
        soPlayer.soPlayerHealth.DamageHealthChangeEvent.AddListener(Damaged);
        soPlayer.soPlayerHealth.HealHealthChangeEvent.AddListener(Heal);
        soSave.RestartEvent.AddListener(Restart);
        soPlayer.LevelUpEvent.AddListener(SetConfiguration);
        soPlayer.soPlayerHealth.DieEvent.AddListener(OnDie);
        soPlayer.StartCutsceneEvent.AddListener(InCutscene);
        soPlayer.EndCutsceneEvent.AddListener(OutCutscene);
    }
    public void OnDisable()
    {
        //soPlayer.soPlayerHealth.DamageHealthChangeEvent.RemoveListener(DamagedCooldown);
        soPlayer.soPlayerHealth.DamageHealthChangeEvent.RemoveListener(Damaged);
        soPlayer.soPlayerHealth.HealHealthChangeEvent.RemoveListener(Heal);
        soSave.RestartEvent.RemoveListener(Restart);
        soPlayer.LevelUpEvent.RemoveListener(SetConfiguration);
        soPlayer.soPlayerHealth.DieEvent.RemoveListener(OnDie);
        soPlayer.StartCutsceneEvent.RemoveListener(InCutscene);
        soPlayer.EndCutsceneEvent.RemoveListener(OutCutscene);
    }
}
