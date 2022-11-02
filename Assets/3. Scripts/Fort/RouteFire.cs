using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFire : MonoBehaviour
{
    public Transform target;

    public float vel;
    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, target.position, Time.deltaTime * vel);
    }
}
