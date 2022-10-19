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
    public GameObject triggers;
    [HideInInspector]
    public List<GameObject> trigger;

    bool completed;

    [HideInInspector]
    public SOTrail soTrail;
    public SOSave soSave;
    public SOFort soFort;
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
        door.Clear();
        cage.Clear();
        trigger.Clear();

        for(int i = 0; i < doors.transform.childCount; i++) {
            door.Add(doors.transform.GetChild(i).gameObject);
            if(this.gameObject.tag != "Fort") door[i].GetComponent<BlueFireWall>().EndFire();
        }

        for(int i = 0; i < cages.transform.childCount; i++) {
            cage.Add(cages.transform.GetChild(i).gameObject);
            cage[i].SetActive(true);
        }

        for(int i = 0; i < triggers.transform.childCount; i++) {
            trigger.Add(triggers.transform.GetChild(i).gameObject);
            trigger[i].SetActive(true);
        }
    }

    void FinishTrail()
    {
        foreach(GameObject d in door)
        {
            d.GetComponent<BlueFireWall>().EndFire();
        }

        foreach(GameObject t in trigger)
        {
            t.SetActive(false);
        }

        completed = true;
        soFort.CompleteSpace();
    }
    void StartTrail()
    {
        foreach(GameObject d in door)
        {
            d.GetComponent<BlueFireWall>().StartFire();
        }
    }

    void CompleteAnticipated()
    {
        if(!completed && gameObject.tag != "Fort")
        {
            foreach(GameObject t in trigger)
            {
                t.SetActive(false);
            }

            foreach(GameObject d in door)
            {
                d.GetComponent<BlueFireWall>().EndFire();
            }

            foreach(GameObject c in cage)
            {
                c.SetActive(false);
            }
        }
    }

    void CompleteFort()
    {
        if(!completed && gameObject.tag == "Fort")
        {
            foreach(GameObject t in trigger)
            {
                t.SetActive(false);
            }

            foreach(GameObject d in door)
            {
                d.GetComponent<BlueFireWall>().EndFire();
            }

            foreach(GameObject c in cage)
            {
                c.SetActive(false);
            }
        }
    }

    void Restart()
    {
        if(!completed)
        {
            foreach(GameObject t in trigger)
            {
                t.SetActive(true);
            }

            foreach(GameObject d in door)
            {
                d.GetComponent<BlueFireWall>().EndFire();
            }
        }
    }

    void OnEnable()
    {
        soTrail.FinishTrailEvent.AddListener(FinishTrail);
        soTrail.EnterTrailEvent.AddListener(StartTrail);
        soSave.RestartEvent.AddListener(Restart);
        soFort.CompleteChallengesEvent.AddListener(CompleteAnticipated);
        soFort.CompleteFortEvent.AddListener(CompleteFort);
    }
    void OnDisable()
    {
        soTrail.FinishTrailEvent.RemoveListener(FinishTrail);
        soTrail.EnterTrailEvent.RemoveListener(StartTrail);
        soSave.RestartEvent.RemoveListener(Restart);
        soFort.CompleteChallengesEvent.RemoveListener(CompleteAnticipated);
        soFort.CompleteFortEvent.RemoveListener(CompleteFort);
    }
}
