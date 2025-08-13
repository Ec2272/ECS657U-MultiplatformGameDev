using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player Stats", fileName = "PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth = 100;

    [Header("Heat")]
    public int maxHeat = 10;
    public int currentHeat = 0;


    public void ApplyDamage(int amount) => currentHealth = Mathf.Max(0, currentHealth - amount);
    public void Heal(int amount) => currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
    //if (player is PlayerCombatant pc && pc.stats != null)
    //pc.stats.currentHealth = player.Health;

    
}

