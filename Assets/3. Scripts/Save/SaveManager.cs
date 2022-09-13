using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
    public Transform firstCheckpoint;
    void Start()
    {
        //soSave.EnterSave(firstCheckpoint);
    }

    void Restart()
    {
        soSave.Restart();
    }

    void OnEnable()
    {
        soPlayer.soPlayerHealth.DieEvent.AddListener(Restart);
    }
    void OnDisable()
    {
        soPlayer.soPlayerHealth.DieEvent.RemoveListener(Restart);
    }
}
