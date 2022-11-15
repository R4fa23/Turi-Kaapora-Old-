using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;

    GameObject loadingScene;

    private void Start()
    {
        loadingScene = GameObject.FindGameObjectWithTag("LoadScreen").GetComponent<PainelLoad>().painel;
    }

    public void AlternateScene(string nextScene)
    {
        StartCoroutine(LoadSceneAsync(nextScene));
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

    public void MenuInitial()
    {
        AlternateScene("MenuTest");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneAsync(string sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScene.GetComponent<Animator>().SetTrigger("Load");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }

    }


}
