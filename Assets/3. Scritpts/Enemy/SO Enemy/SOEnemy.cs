using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOEnemy : ScriptableObject
{
    public SOEnemyHealth soEnemyHealth;
    public SOEnemyAttack soEnemyAttack;
    public SOEnemyMove soEnemyMove;
    public enum State { STOPPED, WALKING, ATTACKING }
    public State state = State.STOPPED;

}
