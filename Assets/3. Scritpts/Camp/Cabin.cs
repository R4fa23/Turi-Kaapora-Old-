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
        OnEnable();
        SummonEnemy();
        //soCamp = camp.soCamp;
    }

    void SummonEnemy()
    {
        enemies[index].transform.position = summonPlace.position;
        enemies[index].SetActive(true);
        enemies[index].GetComponent<EnemyManager>().soEnemy.Summon();
        index++;
        if(index >= enemies.Length) index = 0;
    }

    void OnEnable()
    {
        if(firstEnable)
        {
            foreach(GameObject e in enemies)
            {
                e.GetComponent<EnemyManager>().soEnemy.DieEvent.AddListener(SummonEnemy);
                e.SetActive(false);
            }
        }
        firstEnable = true;
    }
    void OnDisable()
    {
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.RemoveListener(SummonEnemy);
            e.SetActive(false);
        }
    }
}
