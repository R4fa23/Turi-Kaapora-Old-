using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float gravity = 2f;
    float turnSmoothTime;
    float sensibility;
    float verticalSpeed;
    CharacterController characterCtrl;
    Vector2 inputValue;
    float turnSmoothVelocity;
    public GameObject animated;
    Animator animator;
    

    PlayerInput pInput;
    InputAction movement;

    public SOPlayer soPlayer;

    Vector2 dir;
    bool walk;
    bool move;
    bool dash;
    bool initialDash;
    bool focusing;
    GameObject targetFocus;
    bool trapped;
    bool special;
    
    void Start()
    {
        animator = animated.GetComponent<Animator>();
        turnSmoothTime = soPlayer.soPlayerMove.rotationVel;
        dir = new Vector2(0, 1);
        characterCtrl = GetComponent<CharacterController>(); 
        pInput = GetComponent<PlayerInput>();
        movement = pInput.actions["Movement"];
        sensibility = soPlayer.soPlayerMove.vel;
    }

    
    void Update()
    {
        if(!soPlayer.soPlayerHealth.dead)
        {
            walk = false;
            if(move) walk = true;

            if(targetFocus != null)
            {
                if(!targetFocus.activeSelf) 
                {
                    focusing = false;
                    targetFocus = null;
                }
            }
            

            Vector3 moveY = Vector3.zero;
            if (characterCtrl.isGrounded) verticalSpeed = 0;
            else verticalSpeed -= gravity;
            moveY.y = verticalSpeed;
            characterCtrl.Move(moveY * Time.deltaTime);

            if(walk || dash)
            {
                if(!dash)
                {
                    dir = movement.ReadValue<Vector2>();
                    sensibility = soPlayer.soPlayerMove.vel;
                }


        
                if(initialDash)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Caipora/Dash", transform.position);
                    initialDash = false;
                    if(movement.ReadValue<Vector2>().magnitude > 0.1f) dir = movement.ReadValue<Vector2>();
                    else dir = new Vector2(transform.forward.x, transform.forward.z);
                }

                if(trapped)
                {
                    dir = Vector2.zero;
                }
                Vector3 playerX;
                inputValue = dir;

                    playerX = new Vector3(inputValue.x, 0, inputValue.y);

                    if (playerX.magnitude > 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(playerX.x, playerX.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                        
                        if(!focusing || dash)
                        {
                            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                            transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        }

                        float superVel;
                        if(soPlayer.soPlayerMove.superVelocity) superVel = 5;
                        else superVel = 1;

                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        RecognizeDirectionAnimation(moveDir);
                        characterCtrl.Move(moveDir.normalized * sensibility * Time.deltaTime * superVel);

                    }
                    
            }
            if(focusing && !dash)
            {
                Vector3 dirP= new Vector3(targetFocus.transform.position.x, transform.position.y, targetFocus.transform.position.z);
                transform.forward = Vector3.RotateTowards(transform.forward, dirP - transform.position, Mathf.PI / 5, 0);
            } 
            move = false;
        }
    }

    public void MoveStart()
    {
        move = true;
    }

    public void MoveEnd()
    {
        //move = false;
    }

    public void RecognizeDirectionAnimation(Vector3 moveDir)
    {
        if(transform.forward.x <= 0)
        {
            if(transform.forward.z <= 0)
            {
                if(moveDir == Vector3.right)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.left)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.up)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.down)
                {
                    animator.SetTrigger("FocEsq");
                }
            
            }
            else
            {
                if(moveDir == Vector3.right)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.left)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.up)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.down)
                {
                    animator.SetTrigger("FocEsq");
                }
            }
        }
        else
        {
            if(transform.forward.z <= 0)
            {
                if(moveDir == Vector3.right)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.left)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.up)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.down)
                {
                    animator.SetTrigger("FocDir");
                }
            }
            else
            {
                if(moveDir == Vector3.right)
                {
                    animator.SetTrigger("FocDir");
                }
                else if(moveDir == Vector3.left)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.up)
                {
                    animator.SetTrigger("FocEsq");
                }
                else if(moveDir == Vector3.down)
                {
                    animator.SetTrigger("FocDir");
                }
            }
        }
    }

    public void DashStart()
    {
        if(soPlayer.state != SOPlayer.State.SPECIAL) soPlayer.state = SOPlayer.State.DASHING;
        dash = true;
        initialDash = true;
        sensibility = soPlayer.soPlayerMove.dashDist/soPlayer.soPlayerMove.dashDuration;
        StartCoroutine(DashDuration(soPlayer.soPlayerMove.dashDuration));
    }

    public void FocusStart(GameObject target)
    {
        focusing = true;
        animator.SetBool("Focus", focusing);
        targetFocus = target;
    }

    public void FocusEnd()
    {
        focusing = false;
        animator.SetBool("Focus", focusing);
        targetFocus = null;
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

    //Listeners para os eventos e funções
    public void OnEnable(){
        soPlayer.soPlayerMove.MoveStartEvent.AddListener(MoveStart);
        soPlayer.soPlayerMove.MoveEndEvent.AddListener(MoveEnd);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(DashStart);
        soPlayer.soPlayerMove.TargetAimEvent.AddListener(FocusStart);
        soPlayer.soPlayerMove.TargetAimStopEvent.AddListener(FocusEnd);
        soPlayer.soPlayerMove.TrappedEvent.AddListener(Trapped);
        soPlayer.soPlayerMove.UntrappedEvent.AddListener(Untrapped);
        soPlayer.soPlayerAttack.SpecialStartEvent.AddListener(SpecialStart);
        soPlayer.soPlayerAttack.SpecialFinishEvent.AddListener(SpecialFinish);
    }
    public void OnDisable(){
        soPlayer.soPlayerMove.MoveStartEvent.RemoveListener(MoveStart);
        soPlayer.soPlayerMove.MoveEndEvent.RemoveListener(MoveEnd);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(DashStart);
        soPlayer.soPlayerMove.TargetAimEvent.RemoveListener(FocusStart);
        soPlayer.soPlayerMove.TargetAimStopEvent.RemoveListener(FocusEnd);
        soPlayer.soPlayerMove.TrappedEvent.RemoveListener(Trapped);
        soPlayer.soPlayerMove.UntrappedEvent.RemoveListener(Untrapped);
        soPlayer.soPlayerAttack.SpecialStartEvent.RemoveListener(SpecialStart);
        soPlayer.soPlayerAttack.SpecialFinishEvent.RemoveListener(SpecialFinish);
    }
    
    IEnumerator DashDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        sensibility = soPlayer.soPlayerMove.vel;
        dash = false;
        if(!trapped && !special)soPlayer.state = SOPlayer.State.STOPPED;
        //if(walk)soPlayer.state = SOPlayer.State.WALKING;
        //if(!walk)soPlayer.state = SOPlayer.State.STOPPED;
    }
    

}
