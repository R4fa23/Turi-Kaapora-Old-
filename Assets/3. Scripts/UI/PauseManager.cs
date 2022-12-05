using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public SOPlayer soPlayer;
    public GameObject panel;
    public GameObject pauseMenu;
    public GameObject configMenu;
    public GameObject cheatMenu;
    public GameObject exitMenu;
    public GameObject videoMenu;
    public GameObject alertMenu;
    bool isPaused;
    public ChangeScene changeScene;
    TargetFrame tf;
    Menus toScreen;
    enum Menus { PAUSE, CONFIG, CHEAT, NONE, EXIT, VIDEO }
    Menus menus;
    
    void Start()
    {
        changeScene = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<ChangeScene>();
        tf = GetComponent<TargetFrame>();
        DisableAll();
    }

    void StartPause()
    {
        if(!isPaused)
        {
            Pause();
            MenuPause();
        }
        else
        {
            if(menus == Menus.PAUSE)
            {
                UnPause();
            }
            else if(menus == Menus.CONFIG)
            {
                MenuPause();
            }
            else if(menus == Menus.CHEAT)
            {
                MenuPause();
            }
            else if(menus == Menus.EXIT)
            {
                MenuPause();
            }
            else if (menus == Menus.VIDEO)
            {
                MenuPause();
            }
        }
    }

    public void MenuPause()
    {
        if (!tf.changed)
        {
            panel.SetActive(true);
            pauseMenu.SetActive(true);
            cheatMenu.SetActive(false);
            configMenu.SetActive(false);
            exitMenu.SetActive(false);
            videoMenu.SetActive(false);
            alertMenu.SetActive(false);
            menus = Menus.PAUSE;
        }
        else
        {
            alertMenu.SetActive(true);
            toScreen = Menus.PAUSE;
        }
    }
    public void MenuConfig()
    {
        if (!tf.changed)
        {
            pauseMenu.SetActive(false);
            videoMenu.SetActive(false);
            configMenu.SetActive(true);
            alertMenu.SetActive(false);
            menus = Menus.CONFIG;
        }
        else
        {
            alertMenu.SetActive(true);
            toScreen = Menus.CONFIG;
        }
    }
    public void MenuCheat()
    {
        pauseMenu.SetActive(false);
        cheatMenu.SetActive(true);
        menus = Menus.CHEAT;
    }
    public void MenuExit()
    {
        pauseMenu.SetActive(false);
        exitMenu.SetActive(true);
        menus = Menus.EXIT;
    }
    public void MenuVideo()
    {
        configMenu.SetActive(false);
        videoMenu.SetActive(true);
        menus = Menus.VIDEO;
    }
    public void SimAlert()
    {
        tf.RevertChanges();
        if (toScreen == Menus.PAUSE) MenuPause();
        else if (toScreen == Menus.CONFIG) MenuConfig();
    }
    public void NaoAlert()
    {
        alertMenu.SetActive(false);
    }
    public void UnPause()
    {
        DisableAll();
        soPlayer.isPaused = false;
        isPaused = false;
        Time.timeScale = 1;
        soPlayer.Unpause();
    }
    public void ExitGame()
    {
        DisableAll();
        Time.timeScale = 1;
        changeScene.MenuInitial();

    }
    public void Pause()
    {
        soPlayer.isPaused = true;
        isPaused = true;
        Time.timeScale = 0;
    }
    void DisableAll()
    {
        panel.SetActive(false);
        pauseMenu.SetActive(false);
        configMenu.SetActive(false);
        cheatMenu.SetActive(false);
        exitMenu.SetActive(false);
        menus = Menus.NONE;
    }

    void OnEnable()
    {
        soPlayer.PauseEvent.AddListener(StartPause);
    }

    void OnDisable()
    {
        soPlayer.PauseEvent.RemoveListener(StartPause);
    }
}
