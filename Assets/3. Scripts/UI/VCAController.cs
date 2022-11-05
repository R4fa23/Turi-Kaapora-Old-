using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VCAController : MonoBehaviour
{
    private FMOD.Studio.VCA VCAMix;
    public string VCAName;
    private Slider slider;

    void Start()
    {
        VCAMix = FMODUnity.RuntimeManager.GetVCA("vca:/" + VCAName);
        slider = GetComponent<Slider>();
    }

    public void AdjustVolume(float volume)
    {
        VCAMix.setVolume(volume);
    }


}
