using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdShadow : MonoBehaviour
{
    public Material[] birds;

    void Start()
    {
        RandomizeDir();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up, Space.World);
    }

    void RandomizeDir()
    {

        int side = 0;
        float lateralOffset = 0;
        float rotationOffset = 0;
        int randomSprite = 0;

        side = Random.Range(0, 4);
        lateralOffset = Random.Range(-100f, 100f);
        rotationOffset = Random.Range(-30f, 30f);
        randomSprite = Random.Range(0, birds.Length);

        GetComponent<MeshRenderer>().material = birds[randomSprite];

        switch (side)
        {
            case 0:
                //traz
                transform.position = new Vector3(lateralOffset, transform.position.y, -100);
                transform.eulerAngles = new Vector3(90, 0, 0 + rotationOffset);
                break;
            case 1:
                //esquerda
                transform.position = new Vector3(-100, transform.position.y, lateralOffset);
                transform.eulerAngles = new Vector3(90, 0, 270 + rotationOffset);
                break;
            case 2:
                //frente
                transform.position = new Vector3(lateralOffset, transform.position.y, 100);
                transform.eulerAngles = new Vector3(90, 0, 180 + rotationOffset);
                break;
            case 3:
                //direita
                transform.position = new Vector3(100, transform.position.y, lateralOffset);
                transform.eulerAngles = new Vector3(90, 0, 90 + rotationOffset);
                break;
        }

        StartCoroutine(timeToRandomize());
    }

    IEnumerator timeToRandomize()
    {
        yield return new WaitForSeconds(5);
        RandomizeDir();
    }
}
