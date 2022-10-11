using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOSave", menuName = "ScriptableObjects/Save")]
public class SOSave : ScriptableObject
{
    [HideInInspector]
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
