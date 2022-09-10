using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOSave : ScriptableObject
{
    public Transform savePoint;

    [System.NonSerialized]
    public UnityEvent EnterSaveEvent;
    [System.NonSerialized]
    public UnityEvent RestartEvent;


    void OnEnable()
    {
        if(EnterSaveEvent == null)
            EnterSaveEvent = new UnityEvent();

        if(RestartEvent == null)
            RestartEvent = new UnityEvent();
    }

    public void EnterSave(Transform point)
    {
        savePoint = point;
    }

    public void Restart()
    {
        RestartEvent.Invoke();
    }
}
