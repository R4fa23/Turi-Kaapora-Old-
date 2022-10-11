using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOFort", menuName = "ScriptableObjects/Level/Fort")]
public class SOFort : ScriptableObject
{
    public int challenges;
    [HideInInspector]
    public int challengesComplete = 0;

    [System.NonSerialized]
    public UnityEvent CompleteSpaceEvent;
    [System.NonSerialized]
    public UnityEvent CompleteChallengesEvent;
    [System.NonSerialized]
    public UnityEvent CompleteFortEvent;


    void OnEnable()
    {
        if(CompleteSpaceEvent == null)
            CompleteSpaceEvent = new UnityEvent();
        
        if(CompleteChallengesEvent == null)
            CompleteChallengesEvent = new UnityEvent();

        if(CompleteFortEvent == null)
            CompleteFortEvent = new UnityEvent();
    }

    public void CompleteSpace()
    {
        challengesComplete++;
        if(challengesComplete >= challenges)
        {
            CompleteChallenges();
        }
    }

    public void CompleteChallenges()
    {
        CompleteChallengesEvent.Invoke();
    }

    public void CompleteFort()
    {
        CompleteFortEvent.Invoke();
    }
}
