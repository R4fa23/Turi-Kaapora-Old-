using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPlayerManager : MonoBehaviour
{
    public SOPlayer soPlayer;

    public int[] maxLifes = new int[3];
    public float[] velBases = new float[3];
    public float[] maxStaminas = new float[3];
    public float[] staminasRecharge = new float[3];
    public int[] attackDamages = new int[3];
    public int[] comboDamages = new int[3];
    public int[] specialDamages = new int[3];
    public float[] specialCooldowns = new float[3];
    public float[] specialCooldownReductions = new float[3];

    
    void Awake()
    {
        SetLevel();
    }

    void SetLevel()
    {
        int index = soPlayer.level;
        soPlayer.soPlayerHealth.maxLife = maxLifes[index];
        soPlayer.soPlayerMove.velBase = velBases[index];
        soPlayer.soPlayerMove.maxStaminas = maxStaminas[index];
        soPlayer.soPlayerMove.rechargeStaminasTime = staminasRecharge[index];
        soPlayer.soPlayerAttack.attackDamage = attackDamages[index];
        soPlayer.soPlayerAttack.comboDamage = comboDamages[index];
        soPlayer.soPlayerAttack.specialDamage = specialDamages[index];
        soPlayer.soPlayerAttack.specialCooldown = specialCooldowns[index];
        soPlayer.soPlayerAttack.cooldownReduction = specialCooldownReductions[index];
    }
}
