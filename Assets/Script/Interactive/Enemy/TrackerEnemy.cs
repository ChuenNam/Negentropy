using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerEnemy : BaseEnemy
{
    public bool alwaysTrack;
    public float checkRange = 8;
    public float trackVelocity = 1;
    public float waitTime = 2;
    
    private bool isTracking;
    private bool isWaiting;

    protected override void OnUpdate()
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
            transform.LookAt(player.transform);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var direction = (player.transform.position - transform.position).normalized;
        transform.position += -direction * trackVelocity * .5f;
        Player.Instance.MinusHP(enemyDamage);
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