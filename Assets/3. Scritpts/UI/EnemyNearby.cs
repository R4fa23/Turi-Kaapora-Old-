using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNearby : MonoBehaviour
{
    public SOPlayer soPlayer;
    public SOSave soSave;
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
        soPlayer.soPlayerMove.NearbyAimEvent.AddListener(EnableImage);
        soPlayer.soPlayerMove.NearbyAimStopEvent.AddListener(DisableImage);
        soSave.RestartEvent.AddListener(DisableImage);
    }
    public void OnDisable()
    {
        soPlayer.soPlayerMove.NearbyAimEvent.RemoveListener(EnableImage);
        soPlayer.soPlayerMove.NearbyAimStopEvent.RemoveListener(DisableImage);
        soSave.RestartEvent.RemoveListener(DisableImage);
    }
}
