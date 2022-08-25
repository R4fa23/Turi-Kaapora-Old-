using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    PlayerMap playerMap;
    void Start()
    {
        playerMap = new PlayerMap();


        playerMap.Default.Enable();
        playerMap.Default.Movement.performed += MovementPerformed;
        playerMap.Default.Movement.canceled += MovementCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovementPerformed(InputAction.CallbackContext context)
    {
        soPlayer.soPlayerMove.MoveStart();
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        soPlayer.soPlayerMove.MoveEnd();
    }
}
