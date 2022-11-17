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
    bool isPaused;
    public ChangeScene changeScene;
    enum Menus { PAUSE, CONFIG, CHEAT, NONE, EXIT }
    Menus menus;
    
    void Start()
    {
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
        }
    }

    public void MenuPause()
    {
        panel.SetActive(true);
        pauseMenu.SetActive(true);
        cheatMenu.SetActive(false);
        configMenu.SetActive(false);
        exitMenu.SetActive(false);
        menus = Menus.PAUSE;
    }
    public void MenuConfig()
    {
        pauseMenu.SetActive(false);
        configMenu.SetActive(true);
        menus = Menus.CONFIG;
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
    public void UnPause()
    {
        DisableAll();
        soPlayer.isPaused = false;
        isPaused = false;
        Time.timeScale = 1;
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
