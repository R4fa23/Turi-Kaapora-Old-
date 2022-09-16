using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    public SOPlayer soPlayer;
    public int damage;
    public float vel;
    public float lifeTime;
    public float slowDuration;

    void FixedUpdate()
    {
        transform.Translate(transform.forward * vel, Space.World);
    }

    void DisableWeb()
    {
        StartCoroutine(DisableTime());
    }

    IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            soPlayer.soPlayerHealth.HealthChange(-damage);
            soPlayer.soPlayerMove.Slowed(slowDuration);
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        DisableWeb();
    }


}
