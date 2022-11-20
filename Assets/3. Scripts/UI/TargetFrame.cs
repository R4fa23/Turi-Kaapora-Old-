using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetFrame : MonoBehaviour
{
    public SOConfig soConfig;
    
    [HideInInspector]public bool changed;
    public enum Rates
    {
        noLimit,
        limit10,
        limit30,
        limit60,
        limit120,
        limit240
    }

    Dictionary<Rates, int> rat = new Dictionary<Rates, int>()
    {
        {Rates.noLimit, 0},
        {Rates.limit10, 10},
        {Rates.limit30, 30},
        {Rates.limit60, 60},
        {Rates.limit120, 120},
        {Rates.limit240, 240}
    };
    public enum Resolutions
    {
        scr800x600,
        scr1024x768,
        scr1152x864,
        scr1280x720,
        scr1280x768,
        scr1280x800,
        scr1280x960,
        scr1280x1024,
        scr1360x768,
        scr1366x768,
        scr1600x900,
        scr1600x1024,
        scr1600x1200,
        scr1680x1050,
        scr1920x1080
    }

    Dictionary<Resolutions, Vector2> res = new Dictionary<Resolutions, Vector2>()
    {
        {Resolutions.scr800x600, new Vector2(800, 600) },
        {Resolutions.scr1024x768, new Vector2(1024, 768) },
        {Resolutions.scr1152x864, new Vector2(1152, 864) },
        {Resolutions.scr1280x720, new Vector2(1280, 720) },
        {Resolutions.scr1280x768, new Vector2(1280, 768) },
        {Resolutions.scr1280x800, new Vector2(1280, 800) },
        {Resolutions.scr1280x960, new Vector2(1280, 960) },
        {Resolutions.scr1280x1024, new Vector2(1280, 1024) },
        {Resolutions.scr1360x768, new Vector2(1360, 768) },
        {Resolutions.scr1366x768, new Vector2(1366, 768) },
        {Resolutions.scr1600x900, new Vector2(1600, 900) },
        {Resolutions.scr1600x1024, new Vector2(1600, 1024) },
        {Resolutions.scr1600x1200, new Vector2(1600, 1200) },
        {Resolutions.scr1680x1050, new Vector2(1680, 1050) },
        {Resolutions.scr1920x1080, new Vector2(1920, 1080) },

    };

    public enum Quality
    {
        low,
        medio,
        high
    }

    Rates rate;
    Rates lastRate;

    Resolutions resolution;
    Resolutions lastResolution;

    Quality quality;
    Quality lastQuality;

    bool vsync;
    bool lastVsync;

    bool fullscreen;
    bool lastFullscreen;

    public TMP_Dropdown fram, reso, qual;
    public Toggle full, vsy;
    void Awake()
    {
        if(PlayerPrefs.GetInt("Inicio") == 0)
        {
            PlayerPrefs.SetInt("Inicio", 1);

            PlayerPrefs.SetInt("Rate", 0);
            PlayerPrefs.SetInt("Resolution", 14);
            PlayerPrefs.SetInt("Quality", 1);
            PlayerPrefs.SetInt("Fullscreen", 1);
            PlayerPrefs.SetInt("Vsync", 1);
        }

        if (!soConfig.firstTime)
        {
            soConfig.rate = PlayerPrefs.GetInt("Rate");
            soConfig.resolution = PlayerPrefs.GetInt("Resolution");
            soConfig.quality = PlayerPrefs.GetInt("Quality");
            if (PlayerPrefs.GetInt("Fullscreen") == 0) soConfig.fullscreen = false;
            if (PlayerPrefs.GetInt("Fullscreen") == 1) soConfig.fullscreen = true;
            if (PlayerPrefs.GetInt("Vsync") == 0) soConfig.vsync = false;
            if (PlayerPrefs.GetInt("Vsync") == 1) soConfig.vsync = true;
        }

        rate = (Rates)soConfig.rate;
        resolution = (Resolutions)soConfig.resolution;
        quality = (Quality)soConfig.quality;
        vsync = soConfig.vsync;
        fullscreen = soConfig.fullscreen;

        lastResolution = resolution;
        lastRate = rate;
        lastFullscreen = fullscreen;
        lastVsync = vsync;
        lastQuality = quality;

        fram.value = (int)rate;
        reso.value = (int)resolution;
        qual.value = (int)quality;
        full.isOn = fullscreen;
        vsy.isOn = vsync;

        if (!soConfig.firstTime)
        {
            soConfig.firstTime = true;
            Application.targetFrameRate = rat[rate];
            Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
            if (vsync) QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;
        }
    }

    private void Update()
    {
        if(rate != lastRate || resolution != lastResolution || fullscreen != lastFullscreen || vsync != lastVsync || quality != lastQuality)
        {
            changed = true;
        }

        if (rate == lastRate && resolution == lastResolution && fullscreen == lastFullscreen && vsync == lastVsync && quality == lastQuality)
        {
            changed = false;
        }
    }

    public void SetChanges()
    {
        if (changed)
        {
            if (rate != lastRate)
            {
                Application.targetFrameRate = (int)rate;
                lastRate = rate;
                soConfig.rate = (int)rate;
                PlayerPrefs.SetInt("Rate", (int)rate); 
            }

            if (resolution != lastResolution || fullscreen != lastFullscreen)
            {
                Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
                lastResolution = resolution;
                lastFullscreen = fullscreen;
                soConfig.resolution = (int)resolution;
                soConfig.fullscreen = fullscreen;
                PlayerPrefs.SetInt("Resolutin", (int)resolution);
                if (fullscreen) PlayerPrefs.SetInt("Fullscreen", 1);
                if (!fullscreen) PlayerPrefs.SetInt("Fullscreen", 0);             
            }

            if (vsync != lastVsync)
            {
                if (vsync) QualitySettings.vSyncCount = 1;
                else QualitySettings.vSyncCount = 0;
                lastVsync = vsync;
                soConfig.vsync = vsync;
                if (vsync) PlayerPrefs.SetInt("Vsync", 1);
                if (!vsync) PlayerPrefs.SetInt("Vsync", 0);
            }

            if(quality != lastQuality)
            {
                lastQuality = quality;
                soConfig.quality = (int)quality;
                PlayerPrefs.SetInt("Quality", (int)quality);
            }

            changed = false;
        }
    }

    public void RevertChanges()
    {
        if (changed)
        {
            if (rate != lastRate)
            {
                rate = lastRate;
                fram.value = (int)rate;
            }

            if (resolution != lastResolution || fullscreen != lastFullscreen)
            {
                resolution = lastResolution;
                fullscreen = lastFullscreen;
                reso.value = (int)resolution;
                full.isOn = fullscreen;
            }

            if (vsync != lastVsync)
            {
                vsync = lastVsync;
                vsy.isOn = vsync;
            }

            if (quality != lastQuality)
            {
                quality = lastQuality;
                qual.value = (int)quality;
            }

            changed = false;
        }
    }


    public void DropRate(int index)
    {
        rate = (Rates)index;
    }

    public void DropRes(int index)
    {
        resolution = (Resolutions)index;
    }

    public void TogFullscreen(bool index)
    {
        fullscreen = index;
    }

    public void TogVsync(bool index)
    {
        vsync = index;
    }

    public void DropQual(int index)
    {
        quality = (Quality)index;
    }
}
