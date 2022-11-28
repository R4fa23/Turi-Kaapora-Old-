using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManagerUI : MonoBehaviour
{
    [SerializeField] Image[] lifeBar;
    [SerializeField] GameObject[] lifeObject;
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
    int offset;
    

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
        soPlayerHealth.DamageHealthChangeEvent.AddListener(UpdateLifeBar);
        soPlayerMove.ChangeMaxStaminaEvent.AddListener(UpdateStaminaCount);
        soPlayer.LevelUpEvent.AddListener(ChangeLifeBar);
    }

    void OnDisable()
    {
        soPlayerHealth.DamageHealthChangeEvent.RemoveListener(UpdateLifeBar);
        soPlayerMove.ChangeMaxStaminaEvent.RemoveListener(UpdateStaminaCount);
        soPlayer.LevelUpEvent.RemoveListener(ChangeLifeBar);
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

        if(currentLife > 60)
        {
            lifeBar[0].fillAmount = 1;
            lifeBar[1].fillAmount = 1;
            lifeBar[2].fillAmount = (currentLife - 60) / (maxLife - 60);
        }
        else if(currentLife > 50)
        {
            lifeBar[0].fillAmount = 1;
            lifeBar[1].fillAmount = (currentLife - 50) / (maxLife - (50 + offset - 10));
            lifeBar[2].fillAmount = 0;
        }
        else
        {
            lifeBar[0].fillAmount = currentLife / (maxLife - offset);
            lifeBar[1].fillAmount = 0;
            lifeBar[2].fillAmount = 0;
        }

    }

    public void ChangeLifeBar()
    {
        foreach(GameObject i in lifeObject)
        {
            i.SetActive(false);
        }

        offset = -10;
        for (int i = 0; i <= soPlayer.level; i++)
        {
            lifeObject[i].SetActive(true);
            offset += 10;
        }
    }

    public void UpdateStaminaBar()
    {
        int index = (int)soPlayerMove.staminas;

        if(index < soPlayerMove.maxStaminas)
        {
            for(int i = bars.Count - 1; i > index; i--) 
            {
                bars[i].fillAmount = 0;
            }
            bars[index].fillAmount = soPlayerMove.rechargeTime/soPlayerMove.rechargeStaminasTime;

            if (bars[index].fillAmount < 0.97) bars[index].color = Color.gray;
            else bars[index].color = Color.white;
        }

        
    }

    public void UpdateStaminaCount()
    {
        bars = stamina.GetComponent<StaminaBar>().bars;
    }

    public void UpdateSpecialBar()
    {
        specialBar.fillAmount = soPlayerAttack.specialTime/soPlayerAttack.specialCooldown;

        if (specialBar.fillAmount < 1) specialBar.color = Color.gray;
        else specialBar.color = Color.white;
    }
}
