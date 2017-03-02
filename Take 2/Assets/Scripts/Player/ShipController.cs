﻿using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float health;
    public float Health
    {
        set
        {
            health = Mathf.Clamp(value, 0f, maxHealth);
            if (health == 0f)
            {
                OnPlayerDeath();
            }
        }
        get { return health; }
    }

    [SerializeField]
    private int freezeCharges = 1;
    public int FreezeCharges
    {
        set { freezeCharges = value; }
        get { return freezeCharges; }
    }
    [SerializeField]
    private float freezeDuration = 2.0f;
    public float FreezeDuration
    {
        set { freezeDuration = value; }
        get { return freezeDuration; }
    }
    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed
    {
        set { maxSpeed = value; }
        get { return maxSpeed; }
    }

    [SerializeField]
    private uint damage;
    public uint Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    [SerializeField]
    private GameObject shipCamera;

    public static event System.Action OnPlayerDeath;
    public static event System.Action<float> OnPlayerHit;
    public static event System.Action<float> OnFreezeChargeUsed;

    void Start()
    {
        Health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles"))
        {
            //Get bullet damage
            float damage = col.GetComponent<BulletController>().Damage;

            //Remove health from player
            Health = Health - damage;
            OnPlayerHit.Invoke(Health);

            //Destroy Bullet
            col.gameObject.SetActive(false);

            StartCoroutine(shipCamera.GetComponent<CameraShake>().Shake());
        }
    }

    public void UseFreezeCharge()
    {
        if(freezeCharges >0)
        { 
            FreezeCharges--;
            OnFreezeChargeUsed.Invoke(freezeDuration);
        }
        
    }
}
