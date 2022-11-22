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
        if (PlayerPrefs.GetInt("Inicio") == 0)
        {

            PlayerPrefs.SetFloat(VCAName, 1);
        }

        VCAMix = FMODUnity.RuntimeManager.GetVCA("vca:/" + VCAName);
        slider = GetComponent<Slider>();

        VCAMix.setVolume(PlayerPrefs.GetFloat(VCAName));
        slider.value = PlayerPrefs.GetFloat(VCAName);

    }

    public void AdjustVolume(float volume)
    {
        VCAMix.setVolume(volume);
        PlayerPrefs.SetFloat(VCAName, volume);
    }


}
