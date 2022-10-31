using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindPlayerFollow : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public Transform followPoint;
    public bool update;

    private void OnValidate()
    {
        if (update)
        {
            FindPlayer();
            update = false;
        }
    }

    private void Awake()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        if (GameObject.Find("CM Follow").transform != null)
            followPoint = GameObject.Find("CM Follow").transform;
        else followPoint = null;

        if (followPoint)
        {
            virtualCamera.m_Follow = followPoint;
            virtualCamera.m_LookAt = followPoint;
        }
    }
}
