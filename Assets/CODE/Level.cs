using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public string name;

    public List<Wave> waves = new List<Wave>();

    public void Reset()
    {
        foreach (Wave w in waves)
            w.spawned = false;
    }
}

[System.Serializable]
public class Wave
{
    public float spawnTime;
    public List<EnemyTypes> Enemies = new List<EnemyTypes>();
    [HideInInspector]
    public bool spawned;
}
