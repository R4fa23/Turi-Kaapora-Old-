using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNearby : MonoBehaviour
{
    SOEnemy soEnemy;

    bool firstEnable;
    void Start()
    {
        soEnemy = transform.parent.transform.parent.transform.GetComponent<EnemyManager>().soEnemy;
        OnEnable();
    }

    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(transform.parent.transform.parent.transform.position);
    }
    void EnableImage()
    {
        GetComponent<Image>().enabled = true;
    }

    void DisableImage()
    {
        GetComponent<Image>().enabled = false;
    }
    public void OnEnable()
    {
        if(firstEnable)
        {
            soEnemy.StartAimRangeEvent.AddListener(EnableImage);
            soEnemy.EndAimRangeEvent.AddListener(DisableImage);
        }
        firstEnable = true;
    }
    public void OnDisable()
    {
        soEnemy.StartAimRangeEvent.RemoveListener(EnableImage);
        soEnemy.EndAimRangeEvent.RemoveListener(DisableImage);
    }
}
