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
    public enum animal {Tucano, Macaco, Onca, Jacare};
    public animal animals;

    [SerializeField] BoxCollider colliderCage;

    [SerializeField] bool update;


    private void Start()
    {
        animator = GetComponent<Animator>();

        if (update)
        {
            for (int i = 0; i < animalsArray.Length; i++)
            {
                if (animalsArray[i].name == animals.ToString())
                {
                    if (animalsArray[i].name == "Tucano" || animalsArray[i].name == "Macaco")
                    {
                        small.SetActive(true);
                        colliderCage.size = small.transform.localScale;
                        big.SetActive(false);
                    }
                    else
                    {
                        small.SetActive(false);
                        big.SetActive(true);
                        colliderCage.size = big.transform.localScale;
                    }
                    animalsArray[i].SetActive(true);
                }
                else animalsArray[i].SetActive(false);
            }
            update = false;
        }
    }

    private void OnValidate()
    {
        if (update)
        {
            for (int i = 0; i < animalsArray.Length; i++)
            {
                if (animalsArray[i].name == animals.ToString())
                {
                    if (animalsArray[i].name == "Tucano" || animalsArray[i].name == "Macaco")
                    {
                        small.SetActive(true);
                        colliderCage.size = small.transform.localScale;
                        big.SetActive(false);
                    }
                    else
                    {
                        small.SetActive(false);
                        big.SetActive(true);
                        colliderCage.size = big.transform.localScale;
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
        if (cageLife > 0)
        {
            cageLife--;
            //Debug.Log(cageLife);
            animator.SetTrigger("damage");
            if (cageLife <= 0) Break();
        }
    }

    void Break()
    {
        //Debug.Log("breal");
        animator.SetTrigger("open");
        soPlayer.soPlayerAttack.EnemyDie(gameObject);
        soTrail.BreakCage();
    }
}
