﻿using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    private uint damage;
    public uint Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    private Vector3 velocity;
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    private const float maxTime = 2f;
    private float time = 0f;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        time = 0f;
    }

    void Update()
    {
        if (!rigidBody.isKinematic)
        {
            time += Time.deltaTime;

            if (time >= maxTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
