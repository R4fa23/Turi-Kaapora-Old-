using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DephtController : MonoBehaviour
{
    [SerializeField] Volume volumeDepth;
    [SerializeField] Volume volumeInitial;
    [SerializeField] Volume volumeFinal;
    [SerializeField] float interpolation;
    DepthOfField depth;

    [SerializeField] float[] focusDistance;
    [SerializeField] float[] focalLenght;
    [SerializeField] float[] aperture;

    [SerializeField] float index = 0;

    float focusDistanceFinal = 0.1f;
    float focalLenghtFinal = 1f;
    float apertureFinal = 1f;

    bool lerp;

    void Start()
    {
        volumeDepth.profile.TryGet(out depth);
    }

    private void Update()
    {
        if (depth.focusDistance.value != focusDistanceFinal)
        {
            float focusCurrent = Mathf.Lerp(depth.focusDistance.value, focusDistanceFinal, interpolation * Time.deltaTime);
            depth.focusDistance.value = focusCurrent;
        }

        if (depth.focalLength.value != focusDistanceFinal)
        {
            float focalCurrent = Mathf.Lerp(depth.focalLength.value, focalLenghtFinal, interpolation * Time.deltaTime);
            depth.focalLength.value = focalCurrent;
        }
        if (depth.aperture.value != focusDistanceFinal)
        {
            float apertureCurrent = Mathf.Lerp(depth.aperture.value, apertureFinal, interpolation * Time.deltaTime);
            depth.aperture.value = apertureCurrent;
        }

        if (lerp)
        {
            volumeInitial.weight = Mathf.Lerp(volumeInitial.weight, 0, 0.9f * Time.deltaTime);
            volumeFinal.weight = Mathf.Lerp(volumeFinal.weight, 1, 0.9f * Time.deltaTime);

            if (volumeInitial.weight == 1 && volumeFinal.weight == 0) lerp = false;
        }
    }
    public void ChangeVolumes()
    {
        lerp = true;
    }

    public void AdjustValues()
    {
        for (int i = 0; i < focusDistance.Length; i++)
        {
            if(i == index) focusDistanceFinal = focusDistance[i];            
        }

        for (int i = 0; i < focalLenght.Length; i++)
        {
            if (i == index) focalLenghtFinal = focalLenght[i];
        }

        for (int i = 0; i < aperture.Length; i++)
        {
            if (i == index) apertureFinal = aperture[i];
        }
        index++;
    }
}
