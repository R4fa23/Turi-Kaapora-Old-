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
        playerMap.Default.Movement.performed += MovementStarted;
        playerMap.Default.Movement.canceled += MovementCanceled;
        playerMap.Default.Dash.started += DashStarted;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(soPlayer.state);
    }

    public void MovementStarted(InputAction.CallbackContext context)
    {
        if(soPlayer.state == SOPlayer.State.STOPPED)
        {
            soPlayer.state = SOPlayer.State.WALKING;
            soPlayer.soPlayerMove.MoveStart();
            
        }
        else if(soPlayer.state == SOPlayer.State.DASHING)
        {
            soPlayer.soPlayerMove.MoveStart();
        }
    }
    public void MovementCanceled(InputAction.CallbackContext context) {
        if(soPlayer.state == SOPlayer.State.WALKING)
        {
            soPlayer.state = SOPlayer.State.STOPPED;
            soPlayer.soPlayerMove.MoveEnd();
        }
        else if(soPlayer.state == SOPlayer.State.DASHING)
        {
            soPlayer.soPlayerMove.MoveEnd();
        }
        
    }
    public void DashStarted(InputAction.CallbackContext context)
    {
        if(soPlayer.state == SOPlayer.State.WALKING)
        {
            soPlayer.state = SOPlayer.State.DASHING;
            soPlayer.soPlayerMove.DashStart();
        }
    }
    
}
