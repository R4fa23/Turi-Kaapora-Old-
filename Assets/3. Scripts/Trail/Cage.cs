using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [HideInInspector]
    public SOTrail soTrail;
    public SOPlayer soPlayer;
    int cageLife = 3;

    Animator animator;

    [SerializeField] GameObject[] animalsArray;
    public GameObject small;
    public GameObject big;
    public enum animal {tucano, macaco, onca, jacare};
    public animal animals;

    [SerializeField] bool update;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnValidate()
    {
        if (update)
        {
            for (int i = 0; i < animalsArray.Length; i++)
            {
                if (animalsArray[i].name == animals.ToString())
                {
                    if (animalsArray[i].name == "tucano" || animalsArray[i].name == "macaco")
                    {
                        small.SetActive(true);
                        big.SetActive(false);
                    }
                    else if (animalsArray[i].name == "onca" || animalsArray[i].name == "jacare")
                    {
                        small.SetActive(false);
                        big.SetActive(true);
                    }
                    animalsArray[i].SetActive(true);
                }
                else animalsArray[i].SetActive(false);                
            }        
            update = false;
        }
    }

    public void LoseLife()
    {
        cageLife--;
        animator.SetTrigger("damage");
        if (cageLife <= 0) Break();
    }

    void Break()
    {
        soPlayer.soPlayerAttack.EnemyDie(this.gameObject);
        soTrail.BreakCage();
        animator.SetTrigger("open");
    }
}
