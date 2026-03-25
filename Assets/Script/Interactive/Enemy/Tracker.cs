using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public bool alwaysTrack;
    public float checkRange = 8;
    public float trackVelocity = 1;
    public float waitTime = 2;
    
    private bool isTracking;
    private bool isWaiting;
    private BaseEnemy self;
    private Transform player;

    private void Start()
    {
        self = GetComponent<BaseEnemy>();
        player = self.player;
    }

    void Update()
    {
        if (isWaiting) return;
        
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < checkRange)
            isTracking = true;
        else if (!alwaysTrack && isTracking)
            isTracking = false;

        if (isTracking)
        {
            var direction = (player.transform.position - transform.position).normalized;
            if (direction == Vector3.zero)
                return;
            transform.position += direction * trackVelocity * Time.deltaTime;
            transform.LookAt(transform);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var direction = (player.transform.position - transform.position).normalized;
        transform.position += -direction * trackVelocity * .5f;
        Player.Instance.MinusHP(self.enemyDamage);
        StartCoroutine(AttackWait());
    }

    private IEnumerator AttackWait()
    {
        isWaiting = true;
        isTracking = false;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }
}