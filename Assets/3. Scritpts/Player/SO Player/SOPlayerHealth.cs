using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SOPlayerHealth : ScriptableObject
{
    [System.NonSerialized]
    public UnityEvent healthChangeEvent;
}
