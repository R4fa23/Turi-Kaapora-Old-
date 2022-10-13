using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIncendiary : MonoBehaviour
{
    public SOEnemy soEnemy;
    public SOPlayer soPlayer;
    bool firstEnable;
    // Start is called before the first frame update
    void Start()
    {
        soEnemy = GetComponent<EnemyManager>().soEnemy;
        OnEnable();
    }

    void BurnPlayer()
    {
        soPlayer.soPlayerHealth.Burned(3);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Inimigos/Incendiario_Ataque", transform.position);
    }

    void OnEnable()
    {
        if(firstEnable)
        {
            soEnemy.PlayerHitedEvent.AddListener(BurnPlayer);
        }
        firstEnable = true;
    }

    void OnDisable()
    {
        soEnemy.PlayerHitedEvent.RemoveListener(BurnPlayer);
    
    }
}
