using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerMove : ScriptableObject
{
    public float vel;
    public float dashDist;
    public float dashDuration;
    public float dashCooldown;
    public float rotationVel;
    public float focusRange;

    [System.NonSerialized]
    public UnityEvent MoveStartEvent;
    [System.NonSerialized]
    public UnityEvent MoveEndEvent;
    [System.NonSerialized]
    public UnityEvent DashStartEvent;
    [System.NonSerialized]
    public UnityEvent AimStartEvent;
    [System.NonSerialized]
    public UnityEvent AimEndEvent;
    [System.NonSerialized]
    public UnityEvent<GameObject> TargetAimEvent;
    [System.NonSerialized]
    public UnityEvent TargetAimStopEvent;


    private void OnEnable() {
        if(MoveStartEvent == null)
            MoveStartEvent = new UnityEvent();
        
        if(MoveEndEvent == null)
            MoveEndEvent = new UnityEvent();

        if(DashStartEvent == null)
            DashStartEvent = new UnityEvent();

        if(AimStartEvent == null)
            AimStartEvent = new UnityEvent();

        if(AimEndEvent == null)
            AimEndEvent = new UnityEvent();
        
        if(TargetAimEvent == null)
            TargetAimEvent = new UnityEvent<GameObject>();
        
        if(TargetAimStopEvent == null)
            TargetAimStopEvent = new UnityEvent();
    }

    public void MoveStart(){
        MoveStartEvent.Invoke();
    }
    public void MoveEnd(){
        MoveEndEvent.Invoke();
    }
    public void DashStart(){
        DashStartEvent.Invoke();
    }
    public void AimStart(){
        AimStartEvent.Invoke();
    }
    public void AimEnd(){
        AimEndEvent.Invoke();
    }
    public void TargetAim(GameObject target){
        TargetAimEvent.Invoke(target);
    }
    public void TargetAimStop()
    {
        TargetAimStopEvent.Invoke();
    }
}
