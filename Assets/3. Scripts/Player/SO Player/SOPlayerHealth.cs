using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPlayerHealth", menuName = "ScriptableObjects/Characters/Player/Health")]
public class SOPlayerHealth : ScriptableObject
{
    
    public int maxLife = 10;
    public int life;
    [HideInInspector]
    public bool burned;
    [HideInInspector]
    public float fireCharges;
    public int fireDamage;
    public float flameDelay;
    public bool dead;
    [HideInInspector]
    public bool canDamaged;
    public int potionHighLife;
    public int potionMidLife;
    public int potionLowLife;


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
        if(!dead && canDamaged)
        {
            life += amount;
            HealthChangeEvent.Invoke();
            if(life <= 0) Die();
        }
    }

    public void Die()
    {
        Debug.Log("Morrer");
        dead = true;
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
