using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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

    Animator animalAnimator;

    [SerializeField] Vector3 sizeBig;
    [SerializeField] Vector3 sizeSmall;

    [SerializeField] Vector3 target;
    bool move;
    bool dissolve;
    float value = 1;


    public List<float> velocitysWalks;
    public List<float> waitDissolves;
    float velocity;
    float wait;
    Transform animalTransform;

    [SerializeField] SkinnedMeshRenderer animalRenderer;

    private string soundName;

    private void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimal();
        //dissolve = true;
    }

    public void ChangeAnimal()
    {
        switch (animals)
        {
            case animal.Tucano:
                velocity = velocitysWalks[0];
                wait = waitDissolves[0];
                soundName = "Tucano";
                break;
            case animal.Macaco:
                velocity = velocitysWalks[1];
                wait = waitDissolves[1];
                soundName = "Macaco";
                break;
            case animal.Onca:
                velocity = velocitysWalks[2];
                wait = waitDissolves[2];
                soundName = "Onca";
                break;
            case animal.Jacare:
                velocity = velocitysWalks[3];
                wait = waitDissolves[3];
                soundName = "Macaco";
                break;
            default:
                break;
        }

        for (int i = 0; i < animalsArray.Length; i++)
        {
            if (animalsArray[i].name == animals.ToString())
            {
                if (animalsArray[i].name == "Tucano" || animalsArray[i].name == "Macaco")
                {
                    colliderCage.size = sizeSmall;
                    animalAnimator = animalsArray[i].GetComponent<Animator>();
                    small.SetActive(true);
                    big.SetActive(false);
                }
                else
                {
                    animalAnimator = animalsArray[i].GetComponent<Animator>();
                    colliderCage.size = sizeBig;
                    small.SetActive(false);
                    big.SetActive(true);                    
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
        if (move) animalTransform.position += animalTransform.forward * Time.deltaTime * velocity;
        if (dissolve)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Ambiencia/Desvanescer", transform.position);

            value = animalRenderer.material.GetFloat("_Dissolve");

            float velocity = 0.5f;

            value -= value * velocity * Time.deltaTime;
            animalRenderer.material.SetFloat("_Dissolve", value);

            if(value <= 0)
            {
                move = false;
                dissolve = false;
            }
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
        StartCoroutine(WaitTransition());
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animais/Gaiola", transform.position);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Animais/" + eventName, transform.position);
    }

    IEnumerator WaitTransition()
    {
        animalAnimator.SetTrigger("free");

        for (int i = 0; i < animalsArray.Length; i++)
        {
            if (animalsArray[i].name == animals.ToString())
            {
                animalTransform = animalsArray[i].transform;
                Debug.Log(animalTransform);
                animalRenderer = animalsArray[i].GetComponentInChildren<SkinnedMeshRenderer>();
            }
        }
        AnimatorStateInfo stateInfo = animalAnimator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitWhile(() => stateInfo.IsName("Run"));
        value = 1;
        move = true;
        StartCoroutine(WaitDissolve());
        
    }
    IEnumerator WaitDissolve()
    {
        yield return new WaitForSeconds(wait);
        dissolve = true;
    }

}
