using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public SOPlayer soPlayer;
    GameObject[] enemies;
    GameObject nearbyEnemy;
    float nearbyDistance;
    GameObject lastNearbyEnemy;
    GameObject enemyFocused;
    bool entered;

    void Start()
    {
        nearbyDistance = Mathf.Infinity;
        DetectEnemies();
        CheckDistance();
    }

    void Update()
    {
        DetectEnemies();
        CheckDistance();
        if(nearbyDistance < soPlayer.soPlayerMove.focusRange)
        {
                if(lastNearbyEnemy != nearbyEnemy)
                {
                    if(lastNearbyEnemy != null) lastNearbyEnemy.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                    if(nearbyEnemy != enemyFocused)nearbyEnemy.GetComponent<EnemyManager>().soEnemy.StartAimRange();
                    lastNearbyEnemy = nearbyEnemy;
                    entered = true;
                }
        }
        else
        {
            if(entered)
            {
                lastNearbyEnemy = null;
                nearbyEnemy.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                entered = false;
            }
            
        }
        if(enemyFocused != null)
        {
            if(Vector3.Distance(transform.position, enemyFocused.transform.position) >= soPlayer.soPlayerMove.focusRange)
            {
                enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
                enemyFocused = null;
                soPlayer.soPlayerMove.TargetAimStop();
            }
        }
    }

    void ActiveAim()
    {
        if(lastNearbyEnemy != null)
        {
            if(enemyFocused != nearbyEnemy)
            {
                if(enemyFocused != null) enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
                enemyFocused = nearbyEnemy;
                enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                enemyFocused.GetComponent<EnemyManager>().soEnemy.StartAim();
                soPlayer.soPlayerMove.TargetAim(enemyFocused);
            }
            else
            {
                enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
                enemyFocused.GetComponent<EnemyManager>().soEnemy.StartAimRange();
                enemyFocused = null;
                soPlayer.soPlayerMove.TargetAimStop();
            }
        }
    }

    void CheckDistance()
    { 

        foreach(GameObject e in enemies)
        {
            if(Vector3.Distance(transform.position, e.transform.position) < nearbyDistance)
            {
                nearbyDistance = Vector3.Distance(transform.position, e.transform.position);
                nearbyEnemy = e;
            }
            else if(nearbyEnemy == e)
            {
                nearbyDistance = Vector3.Distance(transform.position, e.transform.position);
            } 
        }
    }
    void DetectEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void RestartDetect()
    {
        nearbyEnemy = null;
        lastNearbyEnemy = null;
        nearbyDistance = Mathf.Infinity;
        //enemyFocused = null;
        entered = false;
    }
    public void OnEnable()
    {
        soPlayer.soPlayerMove.AimStartEvent.AddListener(ActiveAim);
        soPlayer.soPlayerAttack.EnemyDieEvent.AddListener(RestartDetect);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerMove.AimStartEvent.RemoveListener(ActiveAim);
        soPlayer.soPlayerAttack.EnemyDieEvent.RemoveListener(RestartDetect);
    }
}
