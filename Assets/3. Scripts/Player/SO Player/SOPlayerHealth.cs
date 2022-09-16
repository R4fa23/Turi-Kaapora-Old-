using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerHealth : ScriptableObject
{
    public int maxLife = 10;
    public int life;
    public bool burned;
    public float fireCharges;
    public int fireDamage;

    [System.NonSerialized]
    public UnityEvent HealthChangeEvent;
    [System.NonSerialized]
    public UnityEvent DieEvent;
    [System.NonSerialized]
    public UnityEvent BurnedEvent;

    private void OnEnable() {
        if(HealthChangeEvent == null)
            HealthChangeEvent = new UnityEvent();

        if(DieEvent == null)
            DieEvent = new UnityEvent();
        
        if(BurnedEvent == null)
            BurnedEvent = new UnityEvent();
    }

    public void RecoverHealth()
    {
        HealthChange(maxLife - life);
    }

    public void HealthChange(int amount)
    {
        life += amount;
        HealthChangeEvent.Invoke();
        if(life <= 0) Die();
    }

    public void Die()
    {
        RecoverHealth();
        DieEvent.Invoke();
    }

    public void Burned(int charges)
    {
        fireCharges = charges;
        burned = true;
        BurnedEvent.Invoke();
    }
}
