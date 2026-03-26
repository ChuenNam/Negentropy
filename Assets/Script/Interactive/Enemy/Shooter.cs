using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float checkRange = 8;
    public float bulletSpeed = 1;
    public int bulletDamage = 10;
    public float shotInterval = 2;

    private GameObject bulletPrefab;
    private BaseEnemy self;
    private Transform player;
    
    private Queue<GameObject> bulletPool = new();
    private float shotTimer;
    private bool hasShot;

    private void Start()
    {
        self = GetComponent<BaseEnemy>();
        player = self.player;
        shotTimer = shotInterval;
        hasShot = false;
    }

    private void Update()
    {
        if (hasShot)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0)
            {
                hasShot = false;
                shotTimer = shotInterval;
            }
        }
        if (Vector3.Distance(player.position, transform.position) < checkRange)
        {
            if (!hasShot)
            {
                Shoot();
                hasShot = true;
            }
        }
    }

    private void Awake()
    {
        bulletPrefab = Resources.Load<GameObject>($"Prefab/Item/Bullet");
        // 预生成一些子弹到对象池
        for (var i = 0; i < 2; i++)
        {
            var bulletObj = Instantiate(bulletPrefab);
            bulletObj.SetActive(false);
            bulletPool.Enqueue(bulletObj);
        }
    }

    private void Shoot()
    {
        var bulletObj = GetBulletFromPool();
        var bullet = bulletObj.GetComponent<Bullet>();
        if (bulletObj != null)
        {
            // 初始化子弹
            bullet.Init(bulletDamage, transform);
            bullet.OnDead += ReturnBulletToPool;
            var direction = (player.position+Vector3.up - transform.position).normalized;
            bullet.vel = direction * (bulletSpeed * Time.fixedDeltaTime);
            
            // 激活子弹
            bulletObj.SetActive(true);
        }
    }

    private GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            var bullet = bulletPool.Dequeue();
            return bullet;
        }
        else
        {
            // 如果对象池为空，创建新的子弹
            var bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            return bullet;
        }
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.GetComponent<Bullet>().OnDead = null;
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}