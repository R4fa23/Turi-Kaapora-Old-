using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOTrail", menuName = "ScriptableObjects/Level/Trail")]
public class SOTrail : ScriptableObject
{
    public int cages;
    public int cageCount;


    [System.NonSerialized]
    public UnityEvent EnterTrailEvent;
    [System.NonSerialized]
    public UnityEvent FinishTrailEvent;
    

    void OnEnable()
    {
        if(EnterTrailEvent == null)
            EnterTrailEvent = new UnityEvent();

        if(FinishTrailEvent == null)
            FinishTrailEvent = new UnityEvent();
    }

    public void EnterTrial()
    {
        EnterTrailEvent.Invoke();
    }

    public void BreakCage()
    {
        cageCount++;
        if(cageCount >= cages) FinishTrial();
    }

    public void FinishTrial()
    {
        FinishTrailEvent.Invoke();
    }
}
