using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTool : MonoBehaviour
{
    public bool withShield;
    public bool withShooter;
    public bool withTracker;


    public void SetBool(int index, bool value)
    {
        switch (index)
        {
            case 0: withShield = value; break;
            case 1: withShooter = value; break;
            case 2: withTracker = value; break;
        }
    }
    
    
    public void GenerateEnemy()
    {
        var enemy = Instantiate(Resources.Load<GameObject>($"Prefab/Item/Enemy"));
        
        if (withShield)
            enemy.AddComponent<Shield>();
        if (withShooter)
            enemy.AddComponent<Shooter>();
        if (withTracker)
            enemy.AddComponent<Tracker>();
        
        enemy.transform.position = Player.Instance.transform.position + Vector3.up;
    }
    
}
