using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneInicial : MonoBehaviour
{
    public ChangeScene cs;
    // Update is called once per frame
    void Update()
    {
        if(GetComponent<VideoPlayer>().isPaused)
        {
            cs.MenuInitial();
        }
    }
}
