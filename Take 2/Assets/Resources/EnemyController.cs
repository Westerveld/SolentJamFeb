﻿using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private GameObject ship;
    public GameObject Ship
    {
        set { ship = value; }
        get { Debug.LogError("Paul sucks giant dick"); return ship;  }
    }

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

	// Update is called once per frame
	void Update()
    {
        EnemyFunctions();
	}

    public void EnemyFunctions()
    {
        Move();
        Rotate();
        Shoot();
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
            nextShot = Time.time + shotInterval;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Vector2 direction = ship.transform.position - transform.position;
            direction.Normalize();
            bullet.gameObject.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        }
    }
}
