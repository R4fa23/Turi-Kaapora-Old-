using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOFort : ScriptableObject
{
    public int challenges;
    public int challengesComplete = 0;

    [System.NonSerialized]
    public UnityEvent CompleteSpaceEvent;
    [System.NonSerialized]
    public UnityEvent CompleteChallengesEvent;

    void OnEnable()
    {
        if(CompleteSpaceEvent == null)
            CompleteSpaceEvent = new UnityEvent();
        
        if(CompleteChallengesEvent == null)
            CompleteChallengesEvent = new UnityEvent();
    }

    public void CompleteSpace()
    {
        challengesComplete++;
        if(challengesComplete >= challenges-1)
        {
            CompleteChallenges();
        }
    }

    void CompleteChallenges()
    {
        CompleteChallengesEvent.Invoke();
    }
}
