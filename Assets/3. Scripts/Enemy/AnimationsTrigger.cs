using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsTrigger : MonoBehaviour
{
    public EnemyAttack ea;
    
    public void StartAttack()
    {
        ea.StartAttack();
    }

    public void Wait()
    {
        ea.Wait();
    }

    public void EndAttack()
    {
        ea.EndAttack();
    }
}
