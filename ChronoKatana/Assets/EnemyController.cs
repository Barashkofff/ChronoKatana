using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyController
{

    private static List<GameObject> enemies = new List<GameObject>();

    public static void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public static List<GameObject> EnemiesList() { return enemies; }

    public static void Clear() { enemies.Clear(); }
    
}
