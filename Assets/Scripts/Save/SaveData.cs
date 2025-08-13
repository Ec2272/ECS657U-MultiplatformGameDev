using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Scene & playerr transform
    public int sceneIndex;
    public float x, y, z;

    // PlayerCombatant state
    public int health;
    public int heat;
    public int speed;

    
    public int facing;
    public int distance;
    //defeated enemies to respawn
    public List<string> defeatedEnemyIds = new List<string>();
}

