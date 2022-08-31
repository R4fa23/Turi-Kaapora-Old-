using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTarget : MonoBehaviour
{
    SOEnemy soEnemy;
    
    bool firstEnable;
    // Start is called before the first frame update
    void Start()
    {
        soEnemy = transform.parent.transform.parent.transform.GetComponent<EnemyManager>().soEnemy;
        OnEnable();
    }

    // Update is called once per frame
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
            soEnemy.StartAimEvent.AddListener(EnableImage);
            soEnemy.EndAimEvent.AddListener(DisableImage);
        }
        firstEnable = true;
    }
    public void OnDisable()
    {
        soEnemy.StartAimEvent.RemoveListener(EnableImage);
        soEnemy.EndAimEvent.RemoveListener(DisableImage);
    }
}
