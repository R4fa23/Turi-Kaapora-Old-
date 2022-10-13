using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOFort soFort;
    public GameObject doors;
    [HideInInspector]
    public List<GameObject> door;
    GameObject[] challenges;
    void Start()
    {
        for(int i = 0; i < doors.transform.childCount; i++) {
            door.Add(doors.transform.GetChild(i).gameObject);
            door[i].GetComponent<BlueFireWall>().StartFire();
        }

        challenges = GameObject.FindGameObjectsWithTag("Challenges");
        soFort.challenges = challenges.Length;
        soFort.challengesComplete = 0;
    }

    void CompleteChallenges()
    {
        foreach(GameObject d in door)
        {
            d.GetComponent<BlueFireWall>().EndFire();

        }
    }

    void OnEnable()
    {
        soFort.CompleteChallengesEvent.AddListener(CompleteChallenges);
    }
    void OnDisable()
    {
        soFort.CompleteChallengesEvent.RemoveListener(CompleteChallenges);
    }
}
