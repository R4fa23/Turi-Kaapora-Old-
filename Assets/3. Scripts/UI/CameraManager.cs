using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] cameras;
    public GameObject[] botoes;
    private void Awake()
    {
        cameras = new CinemachineVirtualCamera[transform.childCount];

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i] = transform.GetChild(i).GetComponent<CinemachineVirtualCamera>();
        }
    }

    public void ChangePriority(int wichCamera)
    {

        for (int i = 0; i < cameras.Length; i++)
        {
            if (i == wichCamera) cameras[i].Priority = 1;
            else cameras[i].Priority = 0;
        }

        /*for (int i = 0; i < botoes.Length; i++)
        {
            if (i == wichCamera) botoes[i].SetActive(true);
            else botoes[i].SetActive(false);
        }*/
    }
}
