using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLumberjack : MonoBehaviour
{
    public SOEnemy soEnemy;
    bool firstEnable;
    // Start is called before the first frame update
    void Start()
    {
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        OnEnable();
    }

    void CanNotDamage()
    {
        soEnemy.canDamaged = false;
    }

    void CanDamage()
    {
        soEnemy.canDamaged = true;
    }

    void OnEnable()
    {
        if(firstEnable)
        {
            soEnemy.AttackStartEvent.AddListener(CanNotDamage);
            soEnemy.AttackEndEvent.AddListener(CanDamage);
        }
        firstEnable = true;
    }

    void OnDisable()
    {
        soEnemy.AttackStartEvent.RemoveListener(CanNotDamage);
        soEnemy.AttackEndEvent.RemoveListener(CanDamage);
    
    }


}
