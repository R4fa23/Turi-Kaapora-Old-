using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlueFireWall : MonoBehaviour
{
    [SerializeField] VisualEffect vfxFire;
    [SerializeField] float wallSize;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] float timer;
    [SerializeField] float rateSequencer;
    //[SerializeField] float rateX;

    float flameRate;
    float smokeRate;
    float cindersRate;
    float decalRate;

    public bool update;
    //float xValue;    
    float sequencerValue;
    float direction;
    bool turningOn;
    bool movingWall;
    bool stayOn;

    void Start()
    {
        GetRates();        
        
        boxCollider.size = new Vector3(wallSize, 3, 1);
        boxCollider.center = new Vector3(boxCollider.size.x / 2, 1.5f, 0);
        update = false;

        rateSequencer = 1 / timer;
        vfxFire.SetFloat("Line End", wallSize);

        StartFire();
    }

    private void OnValidate()
    {
        if (update)
        {
            boxCollider.size = new Vector3(wallSize, 3, 1);
            boxCollider.center = new Vector3(boxCollider.size.x / 2, 1.5f, 0);
            update = false;
        }       
    }

    // Update is called once per frame
    void Update()
    {
        if (movingWall)
        {
            if (turningOn) direction = 1;
            else direction = -1;

            sequencerValue = vfxFire.GetFloat("Line Sequencer");
            sequencerValue += rateSequencer * Time.deltaTime * direction;
            vfxFire.SetFloat("Line Sequencer", sequencerValue);
            //Debug.Log(xValue);
        }

        if (stayOn)
        {
            float random = Random.Range(0, 1f);
            vfxFire.SetFloat("Line Sequencer", random);
            //Debug.Log(random);
        }       
    }
    public void StartFire()
    {
        SetRatesToMax();

        vfxFire.SetFloat("Line Sequencer", 0);

        StartCoroutine(WaitFireMaxDistance());
        GetComponent<BoxCollider>().isTrigger = false;
        turningOn = true;
        movingWall = true;

    }

    IEnumerator WaitFireMaxDistance()
    {
        yield return new WaitUntil(() => sequencerValue >= 1);
        //Debug.Log("End Max");
        movingWall = false;
        stayOn = true;
    }

    public void EndFire()
    {
        stayOn = false;
        turningOn = false;
        movingWall = true;
        vfxFire.SetFloat("Line Sequencer", 1);
        StartCoroutine(WaitFireMinDistance());       
        //Debug.Log("startEnd");
    }      

    IEnumerator WaitFireMinDistance()
    {
        yield return new WaitUntil(() => sequencerValue <= 0);
        //Debug.Log("End");
        GetComponent<BoxCollider>().isTrigger = true;
        SetRatesToZero();
    }





    public void SetRatesToZero()
    {
        vfxFire.SetFloat("Flame Rate", 0);
        vfxFire.SetFloat("Smoke Rate", 0);
        vfxFire.SetFloat("Decal Rate", 0);
        vfxFire.SetFloat("Cinders Rate", 0);
    }
    
    public void SetRatesToMax()
    {
        vfxFire.SetFloat("Flame Rate", flameRate);
        vfxFire.SetFloat("Smoke Rate", smokeRate);
        vfxFire.SetFloat("Decal Rate", decalRate);
        vfxFire.SetFloat("Cinders Rate", cindersRate);
    }


    public void GetRates()
    {
        flameRate = vfxFire.GetFloat("Flame Rate");
        smokeRate = vfxFire.GetFloat("Smoke Rate");
        decalRate = vfxFire.GetFloat("Decal Rate");
        cindersRate = vfxFire.GetFloat("Cinders Rate");
    }
    
}
