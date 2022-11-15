using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTimeScale : MonoBehaviour
{

    void Start()
    {
        RepairTimeOnStart();
    }

    private void RepairTimeOnStart()
    {
        Time.timeScale = 1;
    }
}
