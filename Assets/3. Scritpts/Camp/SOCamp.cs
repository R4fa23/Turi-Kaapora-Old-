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
    bool repeat;

    [System.NonSerialized]
    public UnityEvent EnterCampEvent;
    [System.NonSerialized]
    public UnityEvent ConclusionCampEvent;
    [System.NonSerialized]
    public UnityEvent NextWaveEvent;
    
    void OnEnable()
    {
        if(EnterCampEvent == null)
            EnterCampEvent = new UnityEvent();

        if(ConclusionCampEvent == null)
            ConclusionCampEvent = new UnityEvent();

        if(NextWaveEvent == null)
            NextWaveEvent = new UnityEvent();
    }

    public void EnterCamp()
    {
        EnterCampEvent.Invoke();
    }

    public void DieEnemy()
    {
        killCount++;
        if(killCount >= enemyPerWaves[actualWave])
        {
            killCount = 0;
            do
            {
                actualWave++;
                if(actualWave >= waves)
                {
                    repeat = false;
                }
                else
                {
                    if(enemyPerWaves[actualWave] <= 0) repeat = true;
                    else repeat = false;
                }
            }while(repeat);
            
            NextWave();
            
            
        }
    }

    public void NextWave()
    {
        if(actualWave >= waves)
        {
            ConclusionCampEvent.Invoke();
        }
        else
            NextWaveEvent.Invoke();
    }
}
