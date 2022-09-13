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
    
    //Esse script deve ser o único que usa o enum como condição, ele gerencia o input map
    //e é nele que serão chamados as funções de context

    void Awake()
    {
        soSave.savePoint = transform;
        soPlayer.soPlayerHealth.life = soPlayer.soPlayerHealth.maxLife;
        soPlayer.soPlayerAttack.currentCooldown = soPlayer.soPlayerAttack.attackCooldown;
        soPlayer.soPlayerAttack.currentDamage = soPlayer.soPlayerAttack.attackDamage;
        soPlayer.soPlayerAttack.currentDuration = soPlayer.soPlayerAttack.attackDuration;
        soPlayer.soPlayerAttack.comboIndex = 0;
        soPlayer.state = SOPlayer.State.STOPPED;

        playerMap = new PlayerMap();

        //Forma de chamar as funções sem precisar associar manualmente no inspector
        playerMap.Default.Enable();
        playerMap.Default.Movement.started += MovementStarted;
        playerMap.Default.Movement.canceled += MovementCanceled;
        playerMap.Default.Dash.started += DashStarted;
        playerMap.Default.Attack.started += AttackStarted;
        playerMap.Default.Aim.started += AimStarted;

    }
    
    //O jogador só pode se mover se estiver parado ou se já estiver se movendo
    //O dash tem a prioridade de ações, mas pra usar o jogador precisa estar andando
    //O dash pode interromper o ataque
    //O estado base é parado
    void Update()
    {
        //Debug.Log(soPlayer.state);
        if(movement) MovementPerformed(); //Forma pra que rode todo frame enquanto o botão estiver apertado
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
            soPlayer.state = SOPlayer.State.WALKING;
            soPlayer.soPlayerMove.MoveStart();
            
        }
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        movement = false;

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
        if(canDash && soPlayer.state != SOPlayer.State.TRAPPED)
        {
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

    //----------------------------------------------RESTART------------------------------------------

    void Restart()
    {
        soPlayer.state = SOPlayer.State.STOPPED;
        GetComponent<CharacterController>().enabled = false;
        transform.position = soSave.savePoint.position;
        GetComponent<CharacterController>().enabled = true;
    }


    //-------------------------------------------LISTENER---------------------------------------------
    public void OnEnable()
    {
        soPlayer.soPlayerHealth.HealthChangeEvent.AddListener(DamagedCooldown);
        soSave.RestartEvent.AddListener(Restart);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerHealth.HealthChangeEvent.RemoveListener(DamagedCooldown);
        soSave.RestartEvent.RemoveListener(Restart);
    }
}
