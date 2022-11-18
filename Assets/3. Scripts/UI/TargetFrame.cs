using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrame : MonoBehaviour
{
    bool changed;
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
    void Awake()
    {
        lastResolution = resolution;
        lastRate = rate;
        lastFullscreen = fullscreen;
        lastVsync = vsync;
        lastQuality = quality;

        Application.targetFrameRate = rat[rate];
        Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
        if(vsync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
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
            }

            if (resolution != lastResolution || fullscreen != lastFullscreen)
            {
                Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
                lastResolution = resolution;
                lastFullscreen = fullscreen;
            }

            if (vsync != lastVsync)
            {
                if (vsync) QualitySettings.vSyncCount = 1;
                else QualitySettings.vSyncCount = 0;
                lastVsync = vsync;
            }

            if(quality != lastQuality)
            {
                lastQuality = quality;
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
