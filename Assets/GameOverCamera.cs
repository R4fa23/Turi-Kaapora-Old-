using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCamera : MonoBehaviour
{
    [SerializeField] SOPlayerHealth playerHealth;
    [SerializeField] SOSave saveSO;
    [SerializeField] Animator animator;

    private void OnEnable()
    {
        playerHealth.DieEvent.AddListener(CallFadeIn);
        saveSO.RestartEvent.AddListener(CallFadeOut);
    } 
    private void OnDisable()
    {
        playerHealth.DieEvent.RemoveListener(CallFadeIn);
        saveSO.RestartEvent.RemoveListener(CallFadeOut);
    }

    public void CallFadeIn()
    {
        Debug.Log("Fade In");
        animator.SetTrigger("In");
    }
    public void CallFadeOut()
    {
        Debug.Log("Fade Out");
        animator.SetTrigger("Out");
    }
}
