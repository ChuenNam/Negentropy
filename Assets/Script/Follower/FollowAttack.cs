using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAttack : MonoBehaviour
{
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy");

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Mechanism"))
        {
            Debug.Log("Mechanism");

        }
    }
}
