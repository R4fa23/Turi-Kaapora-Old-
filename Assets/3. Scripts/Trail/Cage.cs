using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] Animator animalAnimator;

    [SerializeField] Vector3 sizeBig;
    [SerializeField] Vector3 sizeSmall;

    [SerializeField] Vector3 target;
    [SerializeField] bool move;

    public float velocity;
    Transform animalTransform;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimal();
    }

    public void ChangeAnimal()
    {
        for (int i = 0; i < animalsArray.Length; i++)
        {
            if (animalsArray[i].name == animals.ToString())
            {
                if (animalsArray[i].name == "Tucano" || animalsArray[i].name == "Macaco")
                {
                    small.SetActive(true);
                    colliderCage.size = sizeSmall;
                    animalAnimator = animalsArray[i].GetComponent<Animator>();
                    big.SetActive(false);
                }
                else
                {
                    small.SetActive(false);
                    big.SetActive(true);
                    animalAnimator = animalsArray[i].GetComponent<Animator>();
                    colliderCage.size = sizeBig;
                }
                animalsArray[i].SetActive(true);
            }
            else animalsArray[i].SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (update)
        {
            ChangeAnimal();
            update = false;
        }
    }

    private void Update()
    {
        if (move)
        {
            animalTransform.position += animalTransform.forward * Time.deltaTime * velocity;
            //animalTransform.position = Vector3.MoveTowards(animalTransform.position, target, velocity * Time.deltaTime);
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
        FreeAnimal();
    }

    public void FreeAnimal()
    {
        animalAnimator.SetTrigger("free");
        StartCoroutine(WatiTransition());
    }

    IEnumerator WatiTransition()
    {
        for (int i = 0; i < animalsArray.Length; i++)
        {
            if (animalsArray[i].name == animals.ToString())
            {
                animalTransform = animalsArray[i].transform;
                Debug.Log(animalTransform);              
                
            }
        }

        yield return new WaitWhile(() => animalAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f);
        Debug.Log("Walking");        
        move = true;
    }
}
