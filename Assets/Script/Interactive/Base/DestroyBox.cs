using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.MinusHP(1000);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<BaseObject>().TakeDamage(9999);
        }
    }
}
