using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFrame : MonoBehaviour
{
    public enum Rates
    {
        noLimit = 0,
        limit10 = 10,
        limit30 = 30,
        limit60 = 60,
        limit120 = 120,
        limit240 = 240
    }

    public Rates rate;
    Rates lastRate;

    public Resolutions resolution;
    Resolutions lastResolution;

    public bool vsync;
    bool lastVsync;

    public bool fullscreen;
    bool lastFullscreen;

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


    void Awake()
    {
        lastResolution = resolution;
        lastRate = rate;
        lastFullscreen = fullscreen;
        lastVsync = vsync;

        Application.targetFrameRate = (int)rate;
        Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
        if(vsync) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        if(rate != lastRate)
        {
            Application.targetFrameRate = (int)rate;
            lastRate = rate;
        }

        if(resolution != lastResolution)
        {
            Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
            lastResolution = resolution;
        }

        if(fullscreen != lastFullscreen)
        {
            Screen.SetResolution((int)res[resolution].x, (int)res[resolution].y, fullscreen);
            lastFullscreen = fullscreen;
        }

        if(vsync != lastVsync)
        {
            if (vsync) QualitySettings.vSyncCount = 1;
            else QualitySettings.vSyncCount = 0;
            lastVsync = vsync;
        }
    }
}
