using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float sensibility;
    public float gravity = 2f;
    float verticalSpeed;
    CharacterController characterCtrl;
    Vector2 inputValue;
    float turnSmoothVelocity;
    public float turnSmoothTime;

    PlayerInput pInput;
    InputAction movement;
    PlayerMap playerMap;

    public SOPlayer soPlayer;

    
    // Start is called before the first frame update
    void Start()
    {
        characterCtrl = GetComponent<CharacterController>(); 
        playerMap = new PlayerMap();
        pInput = GetComponent<PlayerInput>();
        movement = pInput.actions["Movement"];
        //playerMap.Default.Enable();
        //playerMap.Default.Movement.performed += Movement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 dir)
    {
        Vector3 playerX;
        inputValue = dir;

            playerX = new Vector3(inputValue.x, 0, inputValue.y);

            Vector3 moveY = Vector3.zero;
            if (characterCtrl.isGrounded) verticalSpeed = 0;
            else verticalSpeed -= gravity;
            moveY.y = verticalSpeed;
            characterCtrl.Move(moveY * Time.deltaTime);

            if (playerX.magnitude > 0.1f)
            {
                float targetAngle = Mathf.Atan2(playerX.x, playerX.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterCtrl.Move(moveDir.normalized * sensibility *Time.deltaTime);
            }
    }

    public void OnEnable(){
        soPlayer.soPlayerMove.MoveEvent.AddListener(Move);
    }
    public void OnDisable(){
        soPlayer.soPlayerMove.MoveEvent.RemoveListener(Move);
    }
    

}
