using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;

    public void AlternateScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            AlternateScene(scene);
        }
        
    }

    public void Level00()
    {
        AlternateScene("Level-00");
    }
    public void Level01()
    {
        AlternateScene("Level-01");
    }

    public void ExitGame()
    {
        Application.Quit();
    }



}
