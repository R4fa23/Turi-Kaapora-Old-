using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerMove : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent<Vector2> MoveEvent;

    private void OnEnable() {
        if(MoveEvent == null)
            MoveEvent = new UnityEvent<Vector2>();
    }

    public void Move(Vector2 dir){
        MoveEvent.Invoke(dir);
    }
}
