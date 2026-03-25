using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float size;
    public Action<GameObject> OnHit;
    public Action<GameObject> OnDead;

    [HideInInspector]public Vector3 vel;
    public float maxLife = 5;
    private float life;
    
    public void Init(int dmg, Transform from, float s = .5f)
    {
        damage = dmg;
        size = s;
        life = maxLife;
        transform.localScale = Vector3.one * size;
        transform.position = from.position;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && life > 0)
        {
            life -= Time.deltaTime;
            transform.Translate(vel);
            if (life <= 0)
            {
                OnDead?.Invoke(gameObject);
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.MinusHP(damage);
        }
        OnHit?.Invoke(other.gameObject);
        OnDead?.Invoke(gameObject);
    }
}