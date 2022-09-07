using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTarget : MonoBehaviour
{
    public SOPlayer soPlayer;
    GameObject target;
    void Start()
    {
        
    }

    void Update()
    {
        if(target != null)transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
    }
    void EnableImage(GameObject aim)
    {
        target = aim;
        GetComponent<Image>().enabled = true;
    }

    void DisableImage()
    {
        GetComponent<Image>().enabled = false;
    }
    public void OnEnable()
    {
        soPlayer.soPlayerMove.TargetAimEvent.AddListener(EnableImage);
        soPlayer.soPlayerMove.TargetAimStopEvent.AddListener(DisableImage);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerMove.TargetAimEvent.RemoveListener(EnableImage);
        soPlayer.soPlayerMove.TargetAimStopEvent.RemoveListener(DisableImage);
    }
}
