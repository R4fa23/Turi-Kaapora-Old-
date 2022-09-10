using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    int cagesQuantity;
    public GameObject[] cages;
    public GameObject[] doors;

    bool completed;

    [HideInInspector]
    public SOTrail soTrail;
    public SOSave soSave;

    void Awake()
    {
        cagesQuantity = cages.Length;
        soTrail = (SOTrail)ScriptableObject.CreateInstance(typeof(SOTrail));
        soTrail.cages = cagesQuantity;

        foreach(GameObject c in cages)
        {
            c.GetComponent<Cage>().soTrail = soTrail;
        }

    }

    void FinishTrail()
    {
        foreach(GameObject d in doors)
        {
            d.SetActive(false);
        }
        completed = true;
    }
    void StartTrail()
    {
        foreach(GameObject d in doors)
        {
            d.SetActive(true);
        }
    }

    void Restart()
    {

    }

    void OnEnable()
    {
        soTrail.FinishTrailEvent.AddListener(FinishTrail);
        soTrail.EnterTrailEvent.AddListener(StartTrail);
        soSave.RestartEvent.AddListener(Restart);
    }
    void OnDisable()
    {
        soTrail.FinishTrailEvent.RemoveListener(FinishTrail);
        soTrail.EnterTrailEvent.RemoveListener(StartTrail);
        soSave.RestartEvent.RemoveListener(Restart);
    }
}
