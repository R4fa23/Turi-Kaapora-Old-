using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Saw : MonoBehaviour
{
    public SOPlayer soPlayer;
    public int damage;
    public float distance;
    public float vel;
    Vector3 firstLocal;
    Vector3 finalLocal;
    Vector3 target;
    bool started;
    bool going;
    public Animator animator;
    public GameObject rail;
    public GameObject saw;
    
    [SerializeField] VisualEffect vfx;
    float direction;

    void Start()
    {
        saw.GetComponent<TriggerDamage>().damage = damage;
        saw.GetComponent<TriggerDamage>().soPlayer = soPlayer;
        going = true;
        started = true;
        firstLocal = transform.position;
        finalLocal = transform.position + (transform.forward * distance);
        target = finalLocal;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + (transform.forward * 10), Color.red);
        saw.transform.position = Vector3.MoveTowards(saw.transform.position, target, vel * Time.deltaTime);

        if(transform.position == target)
        {
            if(target == firstLocal)
            {
                going = true;
                target = finalLocal;
            } 
            else if(target == finalLocal)
            {
                going = false;
                target = firstLocal;
            } 
            animator.SetBool("Going", going);
        }

        
        direction = vfx.GetFloat("Direction");

        if (going) direction = 1;
        else direction = -1;

        vfx.SetFloat("Direction", direction);
    }

    void OnDrawGizmos()
    {
        if(!started)
        {
            Vector3 point = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(point, point + (transform.forward * distance));
        }
    }

    void OnValidate()
    {
        rail.transform.localScale = new Vector3(rail.transform.localScale.x * distance, rail.transform.localScale.y, rail.transform.localScale.z);
    }

    IEnumerator aaa()
    {


        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length > 1);
        
    }
}
