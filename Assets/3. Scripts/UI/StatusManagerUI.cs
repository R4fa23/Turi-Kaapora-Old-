using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManagerUI : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    [SerializeField] Image staminaBar;
    PlayerManager playerManager;
    SOPlayerHealth soPlayerHealth;
    float maxLife;
    float currentLife;
    float maxStamina;
    float currentStamina;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        soPlayerHealth = playerManager.soPlayer.soPlayerHealth;
    }

    private void OnEnable()
    {
        soPlayerHealth.HealthChangeEvent.AddListener(UpdateLifeBar);
    }

    private void Update()
    {
        UpdateLifeBar();
        UpdateStaminaBar();
    }

    public void UpdateLifeBar()
    {
        maxLife = soPlayerHealth.maxLife;
        currentLife = soPlayerHealth.life;
        lifeBar.fillAmount = currentLife / maxLife;
    }

    public void UpdateStaminaBar()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
