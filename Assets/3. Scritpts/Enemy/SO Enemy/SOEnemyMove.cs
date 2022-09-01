using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOEnemyMove : ScriptableObject
{
    public float vel;
    public float distanceDetectation;
    
    [System.NonSerialized]
    public UnityEvent MoveStartEvent;

    private void OnEnable() {
        if(MoveStartEvent == null)
            MoveStartEvent = new UnityEvent();
    }
    public void MoveStart(){
        MoveStartEvent.Invoke();
    }
}
