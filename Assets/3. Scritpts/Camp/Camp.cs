using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour
{
    public int waves;
    public int[] enemyPerWave;

    [HideInInspector]
    public SOCamp soCamp;
    bool completed;
    public GameObject[] doors;
    public GameObject[] cabin;
    public GameObject[] firstEnemies;
    void Awake()
    {
        enemyPerWave = new int[waves];
        soCamp = (SOCamp)ScriptableObject.CreateInstance(typeof(SOCamp));
        SetConfiguration();
    }

    void SetConfiguration()
    {
        soCamp.waves = waves;
        soCamp.enemyPerWaves = enemyPerWave;
    }

    void StartCamp()
    {
        foreach(GameObject d in doors)
        {
            d.SetActive(true);
        }
    }

    public void OnEnable()
    {
        soCamp.EnterCampEvent.AddListener(StartCamp);
        //foreach
    }
    public void OnDisable()
    {
        soCamp.EnterCampEvent.RemoveListener(StartCamp);
    }
}
