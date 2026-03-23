using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    public float size;
    
    public void Init(int dmg, float s)
    {
        damage = dmg;
        size = s;
        transform.localScale = new Vector3(size, .1f, size);
        gameObject.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
            Player.Instance.MinusHP(damage);
        }
    }
}
