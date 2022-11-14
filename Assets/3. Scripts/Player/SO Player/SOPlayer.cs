using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOPlayer", menuName = "ScriptableObjects/Characters/Player/Manager")]
public class SOPlayer : ScriptableObject
{
    [HideInInspector]
    public int level = 0;
    [HideInInspector]
    public bool canTeleport;
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public bool isCutscene;

    public SOPlayerHealth soPlayerHealth;
    public SOPlayerMove soPlayerMove;
    public SOPlayerAttack soPlayerAttack;
    public enum State { STOPPED, WALKING, DASHING, ATTACKING, TRAPPED, SPECIAL}
    public State state = State.STOPPED;

    //------------------------------------------------------------------------------------------------------------
    [System.NonSerialized]
    public UnityEvent LevelUpEvent;
    [System.NonSerialized]
    public UnityEvent PauseEvent;

    private void OnEnable() {
        if(LevelUpEvent == null)
            LevelUpEvent = new UnityEvent();

        if(PauseEvent == null)
            PauseEvent = new UnityEvent();
    }

    public void LevelUp()
    {
        level++;
        if(level > 2) LevelReset();
        else LevelUpEvent.Invoke();
    }

    public void SetLevel(int levelSet)
    {
        if(levelSet > 2) levelSet = 2;
        level = levelSet;
        LevelUpEvent.Invoke();
    }

    public void LevelReset()
    {
        level = 0;
        LevelUpEvent.Invoke();
    }

    public void Pause()
    {
        PauseEvent.Invoke();
    }

}
