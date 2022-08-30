using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerHealth : ScriptableObject
{
    public int maxLife;
    public int life;
    [System.NonSerialized]
    public UnityEvent HealthChangeEvent;
    [System.NonSerialized]
    public UnityEvent DieEvent;
    private void OnEnable() {
        if(HealthChangeEvent == null)
            HealthChangeEvent = new UnityEvent();

        if(DieEvent == null)
            DieEvent = new UnityEvent();
    }

    public void HealthChange(int amount)
    {
        life += amount;
        HealthChangeEvent.Invoke();
        if(life <= 0) Die();
    }

    public void Die()
    {
        DieEvent.Invoke();
    }

}
