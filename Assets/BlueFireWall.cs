using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlueFireWall : MonoBehaviour
{
    [SerializeField] VisualEffect vfxFire;
    [SerializeField] float rate;
    [SerializeField] float wallSize;
    float xValue;    
    bool turnOn;
    bool turnOff;
    void Start()
    {
        StartFire();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOn)
        {
            xValue = vfxFire.GetFloat("Line End");
            xValue += rate * Time.deltaTime;
            vfxFire.SetFloat("Line End", xValue);
            Debug.Log(xValue);
        }
        
        if (turnOff)
        {
            xValue = vfxFire.GetFloat("Line End");
            xValue -= rate * Time.deltaTime;
            vfxFire.SetFloat("Line End", xValue);
            Debug.Log(xValue);
        }
    }

    IEnumerator FireTimerOn()
    {
        yield return new WaitUntil(() => xValue >= wallSize);
        Debug.Log("End");
        turnOn = false;
    }
    IEnumerator FireTimerOff()
    {
        yield return new WaitUntil(() => xValue <= 0);
        Debug.Log("End");
        turnOff = false;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public void StartFire()
    {
        StartCoroutine(FireTimerOn());
        turnOn = true;
        GetComponent<BoxCollider>().isTrigger = false;
    }
    public void EndFire()
    {
        StartCoroutine(FireTimerOff());
        turnOff = true;        
    }
}
