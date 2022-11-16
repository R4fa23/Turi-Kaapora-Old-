using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintFunctions : MonoBehaviour
{
    Animator anim;

    public SOPlayer soPlayer;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Out()
    {
        anim.SetTrigger("Out");
    }

    public void StartHint()
    {
        soPlayer.isCutscene = true;
    }

    public void EndHint()
    {
        soPlayer.isCutscene = false;
        gameObject.SetActive(false);
    }
}
