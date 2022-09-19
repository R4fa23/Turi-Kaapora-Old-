using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPlayer", menuName = "ScriptableObjects/Characters/Player/Manager")]
public class SOPlayer : ScriptableObject
{
    [HideInInspector]
    public int level = 0;

    public SOPlayerHealth soPlayerHealth;
    public SOPlayerMove soPlayerMove;
    public SOPlayerAttack soPlayerAttack;
    public enum State { STOPPED, WALKING, DASHING, ATTACKING, TRAPPED, SPECIAL}
    public State state = State.STOPPED;

    //------------------------------------------------------------------------------------------------------------
    [System.NonSerialized]
    public UnityEvent LevelUpEvent;

    private void OnEnable() {
        if(LevelUpEvent == null)
            LevelUpEvent = new UnityEvent();
    }

    public void LevelUp()
    {
        level++;
        if(level > 2) LevelReset();
        else LevelUpEvent.Invoke();
    }

    public void LevelReset()
    {
        level = 0;
        LevelUpEvent.Invoke();
    }

}
