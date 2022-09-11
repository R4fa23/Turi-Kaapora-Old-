using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabin : MonoBehaviour
{
    public GameObject[] enemies;
    SOCamp soCamp;
    public Camp camp;
    int index;
    public Transform summonPlace;
    bool firstEnable;

    void Start()
    {
        soCamp = camp.soCamp;
        foreach(GameObject e in enemies)
        {
            e.SetActive(false);
        }        
    }

    public void Restart()
    {
        foreach(GameObject e in enemies)
            {
                e.SetActive(false);
            }
    }
    public void SummonEnemy()
    {
        enemies[index].transform.position = summonPlace.position;
        enemies[index].SetActive(true);
        enemies[index].GetComponent<EnemyManager>().soEnemy.Summon();
        index++;
        if(index >= enemies.Length) index = 0;
    }

    void EnemyDied()
    {
        soCamp.DieEnemy();
    }

    void OnEnable()
    {        
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.AddListener(EnemyDied);
        }        
    }
    void OnDisable()
    {
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.RemoveListener(EnemyDied);
        }
    }
}
