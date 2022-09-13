using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
    public int damage;
    public int clicksToScape;
    public float clickCount;
    bool trapped;
    void Start()
    {
        clickCount = 0;
    }

    void Update()
    {
        if(trapped)
        {
            clickCount -= Time.deltaTime;
        }
        if(clickCount < 0) clickCount = 0;
        

    }

    void TrappedClick()
    {
        clickCount++;
        if(clickCount >= clicksToScape) BreakTrap();
    }

    void BreakTrap()
    {
        gameObject.SetActive(false);
        soPlayer.state = SOPlayer.State.STOPPED;
        soPlayer.soPlayerMove.Untrapped();
    }

    void Restart()
    {
        soPlayer.state = SOPlayer.State.STOPPED;
        soPlayer.soPlayerMove.Untrapped();
        trapped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            trapped = true;
            soPlayer.state = SOPlayer.State.TRAPPED;
            soPlayer.soPlayerHealth.HealthChange(-damage);
            soPlayer.soPlayerMove.Trapped();
        }
    }

    void OnEnable()
    {
        soPlayer.soPlayerMove.TrappedClickEvent.AddListener(TrappedClick);
        soSave.RestartEvent.AddListener(Restart);

    }
    void OnDisable()
    {
        soPlayer.soPlayerMove.TrappedClickEvent.RemoveListener(TrappedClick);
        soSave.RestartEvent.RemoveListener(Restart);
    }
}
