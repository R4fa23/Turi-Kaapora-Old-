using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public SOPlayer soPlayer;
    GameObject[] enemies;
    GameObject[] cages;
    List<GameObject> targets;
    GameObject nearbyEnemy;
    float nearbyDistance;
    GameObject lastNearbyEnemy;
    GameObject enemyFocused;
    bool entered;

    float cooldownUpdate = 0.05f;
    void Start()
    {
        targets = new List<GameObject>();
        nearbyDistance = Mathf.Infinity;
        DetectEnemies();
        CheckDistance();
    }

    void Update()
    {
        if(cooldownUpdate <= 0)
        {
            DetectEnemies();
            CheckDistance();
            cooldownUpdate = 0.05f;
        }
        else cooldownUpdate -= Time.deltaTime;
        
        if(nearbyDistance < soPlayer.soPlayerMove.focusRange)
        {
                if(lastNearbyEnemy != nearbyEnemy)
                {
                    if(lastNearbyEnemy != null) soPlayer.soPlayerMove.NearbyAimStop();//lastNearbyEnemy.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                    if(nearbyEnemy != enemyFocused) soPlayer.soPlayerMove.NearbyAim(nearbyEnemy);//nearbyEnemy.GetComponent<EnemyManager>().soEnemy.StartAimRange();
                    lastNearbyEnemy = nearbyEnemy;
                    entered = true;
                }
        }
        else
        {
            if(entered)
            {
                lastNearbyEnemy = null;
                soPlayer.soPlayerMove.NearbyAimStop();//nearbyEnemy.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                entered = false;
            }
            
        }

        if(enemyFocused != null)
        {
            if(Vector3.Distance(transform.position, enemyFocused.transform.position) >= soPlayer.soPlayerMove.focusRange)
            {
                enemyFocused = null;
                soPlayer.soPlayerMove.TargetAimStop();//enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
            }
        }
    }

    void ActiveAim()
    {
        if(lastNearbyEnemy != null)
        {
            if(enemyFocused != nearbyEnemy)
            {
                if(enemyFocused != null) soPlayer.soPlayerMove.TargetAimStop();//enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
                enemyFocused = nearbyEnemy;
                soPlayer.soPlayerMove.NearbyAimStop();//enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAimRange();
                soPlayer.soPlayerMove.TargetAim(enemyFocused);//enemyFocused.GetComponent<EnemyManager>().soEnemy.StartAim();
            }
            else
            {
                soPlayer.soPlayerMove.TargetAimStop();//enemyFocused.GetComponent<EnemyManager>().soEnemy.EndAim();
                soPlayer.soPlayerMove.NearbyAim(enemyFocused);//enemyFocused.GetComponent<EnemyManager>().soEnemy.StartAimRange();
                enemyFocused = null;
                
            }
        }
    }

    void CheckDistance()
    { 

        foreach(GameObject e in targets)
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
        enemies = GameObject.FindGameObjectsWithTag("EnemyTarget");
        cages = GameObject.FindGameObjectsWithTag("Cage");
        targets.Clear();

        foreach(GameObject e in enemies)
        {
            targets.Add(e);
        }
        foreach(GameObject c in cages)
        {
            targets.Add(c);
        }

    }

    void RestartDetect(GameObject enemyDied)
    {
        nearbyEnemy = null;
        lastNearbyEnemy = null;
        nearbyDistance = Mathf.Infinity;
        //enemyFocused = null;
        entered = false;
        if(enemyDied == enemyFocused) soPlayer.soPlayerMove.TargetAimStop();
        soPlayer.soPlayerMove.NearbyAimStop();
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
