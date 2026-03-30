using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenByTime : MonoBehaviour
{
    public bool gen = false;
    public GameObject prefab;
    public float shotInterval = 2;
    
    public float shotTimer;
    public bool hasShot;

    private void Start()
    {
        shotTimer = shotInterval;
        hasShot = true;
    }

    private void Update()
    {
        if (gen)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                hasShot = false;
                shotTimer = shotInterval;
            }
        }
        if (!hasShot)
        {
            var obj = Instantiate(prefab, transform.position, transform.rotation,transform);
            hasShot = true;
        }
    }
}
