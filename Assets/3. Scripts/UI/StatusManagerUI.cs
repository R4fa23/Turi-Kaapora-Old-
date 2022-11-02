using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManagerUI : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    [SerializeField] GameObject stamina;
    [SerializeField] Image specialBar;
    PlayerManager playerManager;
    SOPlayer soPlayer;
    SOPlayerHealth soPlayerHealth;
    SOPlayerMove soPlayerMove;
    SOPlayerAttack soPlayerAttack;
    StaminaBar staminaBar;
    float maxLife;
    float currentLife;
    float maxStamina;
    float currentStamina;
    List<Image> bars;
    

    private void Awake()
    {

        playerManager = FindObjectOfType<PlayerManager>();
        soPlayer = playerManager.soPlayer;
        soPlayerHealth = soPlayer.soPlayerHealth;
        soPlayerMove = soPlayer.soPlayerMove;
        soPlayerAttack = soPlayer.soPlayerAttack;
    }

    private void OnEnable()
    {
        soPlayerHealth.HealthChangeEvent.AddListener(UpdateLifeBar);
        soPlayerMove.ChangeMaxStaminaEvent.AddListener(UpdateStaminaCount);
    }

    void OnDisable()
    {
        soPlayerHealth.HealthChangeEvent.RemoveListener(UpdateLifeBar);
        soPlayerMove.ChangeMaxStaminaEvent.RemoveListener(UpdateStaminaCount);
    }

    private void Update()
    {
        UpdateLifeBar();
        UpdateStaminaBar();
        UpdateSpecialBar();
    }

    public void UpdateLifeBar()
    {
        maxLife = soPlayerHealth.maxLife;
        currentLife = soPlayerHealth.life;
        lifeBar.fillAmount = currentLife / maxLife;
    }

    public void UpdateStaminaBar()
    {
        int index = (int)soPlayerMove.staminas;

        if(index < soPlayerMove.maxStaminas)
        {
            for(int i = bars.Count - 1; i > index; i--) {
                bars[i].fillAmount = 0;
            }
            bars[index].fillAmount = soPlayerMove.rechargeTime/soPlayerMove.rechargeStaminasTime;

        }
    }

    public void UpdateStaminaCount()
    {
        bars = stamina.GetComponent<StaminaBar>().bars;
    }

    public void UpdateSpecialBar()
    {
        specialBar.fillAmount = soPlayerAttack.specialTime/soPlayerAttack.specialCooldown;
    }
}
