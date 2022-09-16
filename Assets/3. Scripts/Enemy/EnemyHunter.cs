using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHunter : MonoBehaviour
{
    SOEnemy soEnemy;
    public SOPlayer soPlayer;
    public GameObject web;
    bool firstEnable;
    // Start is called before the first frame update
    void Start()
    {
        web.SetActive(false);
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        OnEnable();
    }

    void LaunchWeb()
    {

        web.SetActive(true);
        web.transform.position = transform.position;
        web.transform.forward = transform.forward;
    }

    void OnEnable()
    {
        if(firstEnable)
        {
            soEnemy.AttackStartEvent.AddListener(LaunchWeb);
        }
        firstEnable = true;
    }

    void OnDisable()
    {
        soEnemy.AttackStartEvent.RemoveListener(LaunchWeb);
    }
}
