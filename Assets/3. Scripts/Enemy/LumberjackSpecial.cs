using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberjackSpecial : MonoBehaviour
{
    public SOPlayer soPlayer;
    SOEnemy soEnemy;
    public GameObject manager;
    void Start()
    {
        soEnemy = manager.GetComponent<EnemyManager>().soEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(-soEnemy.attackDamage * 2);
        }
    }
}
