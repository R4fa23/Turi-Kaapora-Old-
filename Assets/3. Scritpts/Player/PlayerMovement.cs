using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public string description;
    public float gravity = 2f;
    public float turnSmoothTime;
    float sensibility;
    float verticalSpeed;
    CharacterController characterCtrl;
    Vector2 inputValue;
    float turnSmoothVelocity;
    

    PlayerInput pInput;
    InputAction movement;

    public SOPlayer soPlayer;

    Vector2 dir;
    bool walk;
    bool move;
    bool dash;
    bool initialDash;

    
    void Start()
    {
        dir = new Vector2(0, 1);
        characterCtrl = GetComponent<CharacterController>(); 
        pInput = GetComponent<PlayerInput>();
        movement = pInput.actions["Movement"];
        sensibility = soPlayer.soPlayerMove.vel;
    }

    
    void Update()
    {
        walk = false;
        if(move) walk = true;

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
            } 
            if(initialDash)
            {
                initialDash = false;
                if(movement.ReadValue<Vector2>().magnitude > 0.1f) dir = movement.ReadValue<Vector2>();
            }
            Vector3 playerX;
            inputValue = dir;

                playerX = new Vector3(inputValue.x, 0, inputValue.y);

                if (playerX.magnitude > 0.1f)
                {
                    float targetAngle = Mathf.Atan2(playerX.x, playerX.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);

                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    characterCtrl.Move(moveDir.normalized * sensibility *Time.deltaTime);
                }
        }
        move = false;
    }

    public void MoveStart()
    {
        move = true;
    }

    public void MoveEnd()
    {
        //move = false;
    }

    public void DashStart()
    {
        dash = true;
        initialDash = true;
        sensibility = soPlayer.soPlayerMove.dashDist/soPlayer.soPlayerMove.dashDuration;
        StartCoroutine(DashDuration(soPlayer.soPlayerMove.dashDuration));
    }

    //Listeners para os eventos e funções
    public void OnEnable(){
        soPlayer.soPlayerMove.MoveStartEvent.AddListener(MoveStart);
        soPlayer.soPlayerMove.MoveEndEvent.AddListener(MoveEnd);
        soPlayer.soPlayerMove.DashStartEvent.AddListener(DashStart);
    }
    public void OnDisable(){
        soPlayer.soPlayerMove.MoveStartEvent.RemoveListener(MoveStart);
        soPlayer.soPlayerMove.MoveEndEvent.RemoveListener(MoveEnd);
        soPlayer.soPlayerMove.DashStartEvent.RemoveListener(DashStart);
    }
    
    IEnumerator DashDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        sensibility = soPlayer.soPlayerMove.vel;
        dash = false;
        soPlayer.state = SOPlayer.State.STOPPED;
        //if(walk)soPlayer.state = SOPlayer.State.WALKING;
        //if(!walk)soPlayer.state = SOPlayer.State.STOPPED;
    }
    

}
