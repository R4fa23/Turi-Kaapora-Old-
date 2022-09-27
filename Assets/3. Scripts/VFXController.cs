using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{

    [SerializeField] VisualEffect vfx;
    
   

    public void BurstParticles()
    {
        vfx.Play();
    }
}
