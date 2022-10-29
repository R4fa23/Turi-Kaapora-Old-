using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabin : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] enemiesMelee;
    public GameObject[] enemiesRange;
    public GameObject[] enemiesIncendiary;
    public GameObject[] enemiesHunter;
    public GameObject[] enemiesLumberjack;
    SOCamp soCamp;
    public Camp camp;
    public Transform summonPlace;

    void Start()
    {
        soCamp = camp.soCamp;
        StartCoroutine(DisableEnemies());
    }

    public void Restart()
    {
        foreach(GameObject e in enemies)
            {
                e.SetActive(false);
            }
    }
    public void SummonEnemy(string enemyType)
    {
        if(enemyType == "melee")
        {
            if(enemiesMelee[0].gameObject.activeInHierarchy)
            {
                EnemySummoned(enemiesMelee[1]);
            }
            else
            {
                EnemySummoned(enemiesMelee[0]);
            }
        }
        else if(enemyType == "range")
        {
            if(enemiesRange[0].gameObject.activeInHierarchy)
            {
                EnemySummoned(enemiesRange[1]);
            }
            else
            {
                EnemySummoned(enemiesRange[0]);
            }
        }
        else if(enemyType == "incendiary")
        {
            if(enemiesIncendiary[0].gameObject.activeInHierarchy)
            {
                EnemySummoned(enemiesIncendiary[1]);
            }
            else
            {
                EnemySummoned(enemiesIncendiary[0]);
            }
        }
        else if(enemyType == "hunter")
        {
            if(enemiesHunter[0].gameObject.activeInHierarchy)
            {
                EnemySummoned(enemiesHunter[1]);
            }
            else
            {
                EnemySummoned(enemiesHunter[0]);
            }
        }
        else if(enemyType == "lumberjack")
        {
            if(enemiesLumberjack[0].gameObject.activeInHierarchy)
            {
                EnemySummoned(enemiesLumberjack[1]);
            }
            else
            {
                EnemySummoned(enemiesLumberjack[0]);
            }
        }
    }

    void EnemySummoned(GameObject enemy)
    {
        enemy.transform.position = summonPlace.position;
        enemy.SetActive(true);
        enemy.GetComponent<EnemyManager>().soEnemy.Summon();
    }

    void EnemyDied()
    {
        soCamp.DieEnemy();
    }

    IEnumerator DisableEnemies()
    {
        yield return new WaitForSeconds(0.02f);
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.AddListener(EnemyDied);
        }        
        yield return new WaitForSeconds(0.02f);
        foreach(GameObject e in enemies)
        {
            e.SetActive(false);
        }        
    }

    public void DisableAll()
    {
        foreach(GameObject e in enemies)
        {
            e.SetActive(false);
        }
    }

    void OnEnable()
    {   
        /*     
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.AddListener(EnemyDied);
        }       
        */ 
    }
    void OnDisable()
    {
        foreach(GameObject e in enemies)
        {
            e.GetComponent<EnemyManager>().soEnemy.DieEvent.RemoveListener(EnemyDied);
        }
    }
}
