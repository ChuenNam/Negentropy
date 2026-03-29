using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnergyBall : MonoBehaviour
{
    public int setNum = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.SetEP(setNum);
        }
    }


    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = new Color(1f, 0.5f, 0.5f);
    }

    private void OnDrawGizmos()
    {
        var coll = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0.5f, 0.1f, 0.5f, 0.35f);
        Gizmos.DrawCube(transform.position, coll.size);
    }
}
