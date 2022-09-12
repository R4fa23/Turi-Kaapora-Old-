using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    int cagesQuantity;
    public GameObject cages;
    [HideInInspector]
    public List<GameObject> cage;
    public GameObject doors;
    [HideInInspector]
    public List<GameObject> door;

    bool completed;

    [HideInInspector]
    public SOTrail soTrail;
    public SOSave soSave;
    public SOFort soFort;
    public GameObject trigger;
    void Awake()
    {
        SetLists();
        cagesQuantity = cage.Count;
        soTrail = (SOTrail)ScriptableObject.CreateInstance(typeof(SOTrail));
        soTrail.cages = cagesQuantity;

        foreach(GameObject c in cage)
        {
            c.GetComponent<Cage>().soTrail = soTrail;
        }

    }

    void SetLists()
    {
        for(int i = 0; i < doors.transform.childCount; i++) {
            door.Add(doors.transform.GetChild(i).gameObject);
            door[i].SetActive(false);
        }

        for(int i = 0; i < cages.transform.childCount; i++) {
            cage.Add(cages.transform.GetChild(i).gameObject);
        }
    }

    void FinishTrail()
    {
        foreach(GameObject d in door)
        {
            d.SetActive(false);
        }
        completed = true;
        soFort.CompleteSpace();
    }
    void StartTrail()
    {
        foreach(GameObject d in door)
        {
            d.SetActive(true);
        }
    }

    void Restart()
    {
        if(!completed)
        {
            trigger.SetActive(true);

            foreach(GameObject d in door)
            {
                d.SetActive(false);
            }
        }
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
