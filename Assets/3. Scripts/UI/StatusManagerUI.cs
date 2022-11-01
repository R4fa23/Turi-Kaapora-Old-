using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManagerUI : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    [SerializeField] GameObject stamina;
    PlayerManager playerManager;
    SOPlayer soPlayer;
    SOPlayerHealth soPlayerHealth;
    StaminaBar staminaBar;
    float maxLife;
    float currentLife;
    float maxStamina;
    float currentStamina;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        soPlayer = playerManager.soPlayer;
        soPlayerHealth = soPlayer.soPlayerHealth;
        staminaBar = stamina.GetComponent<StaminaBar>();
        staminaBar.soPlayer = soPlayer;
    }

    private void OnEnable()
    {
        soPlayerHealth.HealthChangeEvent.AddListener(UpdateLifeBar);
    }

    void OnDisable()
    {
        soPlayerHealth.HealthChangeEvent.RemoveListener(UpdateLifeBar);
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
        
    }
}
