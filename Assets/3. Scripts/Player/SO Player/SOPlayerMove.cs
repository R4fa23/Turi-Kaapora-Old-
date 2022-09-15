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
    public float staminas;
    public float maxStaminas;
    public float rechargeStaminasTime;
    [HideInInspector]
    public float rechargeTime;

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
    [System.NonSerialized]
    public UnityEvent<GameObject> NearbyAimEvent;
    [System.NonSerialized]
    public UnityEvent NearbyAimStopEvent;
    [System.NonSerialized]
    public UnityEvent TrappedEvent;
    [System.NonSerialized]
    public UnityEvent UntrappedEvent;
    [System.NonSerialized]
    public UnityEvent TrappedClickEvent;
    [System.NonSerialized]
    public UnityEvent ChangeStaminaEvent;


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

        if(NearbyAimEvent == null)
            NearbyAimEvent = new UnityEvent<GameObject>();
        
        if(NearbyAimStopEvent == null)
            NearbyAimStopEvent = new UnityEvent();

        if(TrappedEvent == null)
            TrappedEvent = new UnityEvent();

        if(UntrappedEvent == null)
            UntrappedEvent = new UnityEvent();

        if(TrappedClickEvent == null)
            TrappedClickEvent = new UnityEvent();
        
        if(ChangeStaminaEvent == null)
            ChangeStaminaEvent = new UnityEvent();
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
    public void NearbyAim(GameObject target){
        NearbyAimEvent.Invoke(target);
    }
    public void NearbyAimStop()
    {
        NearbyAimStopEvent.Invoke();
    }
    public void Trapped()
    {
        TrappedEvent.Invoke();
    }
    public void Untrapped()
    {
        UntrappedEvent.Invoke();
    }
    public void TrappedClick()
    {
        TrappedClickEvent.Invoke();
    }
    public void RechargeStamina()
    {
        ChangeStaminaCount(1);
    }
    public void ChangeStaminaCount(int amount)
    {
        staminas += amount;
        if(staminas > maxStaminas) staminas = maxStaminas;

        ChangeStaminaEvent.Invoke();
    }
}
