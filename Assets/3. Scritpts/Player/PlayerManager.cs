using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    PlayerInput pInput;
    PlayerMap playerMap;
    InputAction movement;
    void Start()
    {
        playerMap = new PlayerMap();
        pInput = GetComponent<PlayerInput>();
        movement = pInput.actions["Movement"];


        playerMap.Default.Enable();
        playerMap.Default.Movement.performed += MovementPerformed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovementPerformed(InputAction.CallbackContext context)
    {
        soPlayer.soPlayerMove.Move(movement.ReadValue<Vector2>());
    }
}
