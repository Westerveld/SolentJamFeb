﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private GameObject ship;
    public GameObject Ship
    {
        set { ship = value; }
        get { return ship;  }
    }
    [SerializeField]
    private float damage;
    public float Damage
    {
        set { damage = value; }
        get { return damage; }
    }
    private const uint maxHealth = 10;
    private uint health = maxHealth;

    [SerializeField]
    private float distanceToShip;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float nextShot;

    [SerializeField]
    private float shotInterval;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private float bulletSpeed;
    private bool frozen;

    private BulletPool bp;
    public BulletPool Bp
    {
        set { bp = value; }
        get { return Bp; }
    }

    public bool Frozen
    {
        set
        {
            frozen = value;
        }
        get
        {
            return frozen;
        }
    }

    public static event System.Action OnEnemyDeath;
    public void EnemyFunctions()
    {
        if(!frozen)
        { 
            Move();
            Rotate();
            Shoot();
        }
    }
    
    void Move()
    {
        float distance = Vector2.Distance(ship.transform.position, transform.position);

        if (distance > distanceToShip)
        {
            Vector3 myPosition = transform.position;
            Vector3 direction = ship.transform.position - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.RotateAround(ship.transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    void Rotate()
    {
        Vector2 direction = ship.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    void Shoot()
    {
        if(Time.time > nextShot)
        {
            Vector2 direction = ship.transform.position - transform.position;
            direction.Normalize();

            nextShot = Time.time + shotInterval;
            bp.ActivateBullet(bulletSpawn.position, bulletSpeed, direction, damage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "PlayerProjectile")
        {
            
            //ToDo:
            //Enemy Colliding with PlayerProjectile
            //Return to object pool
        }
        if(col.gameObject.tag == "Asteroid")
        {
            //ToDo:
            //Enemy Colliding With Asteroid
            //Return to object pool
        }
    }

    void Destroyed()
    {
        health = maxHealth;
        this.gameObject.SetActive(false);

    }

}
