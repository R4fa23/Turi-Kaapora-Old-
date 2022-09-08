using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOCamp : ScriptableObject
{
    public int waves;
    public int[] enemyPerWaves;
    public int actualWave = 0;
    public int killCount = 0;

    [System.NonSerialized]
    public UnityEvent EnterCampEvent;
    
    void OnEnable()
    {
        if(EnterCampEvent == null)
            EnterCampEvent = new UnityEvent();
    }

    public void EnterCamp()
    {
        EnterCampEvent.Invoke();
    }
}
