using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string scene;

    public void AlternateScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    void OnTriggerEnter(Collider other)
    {
        AlternateScene(scene);
    }

}
