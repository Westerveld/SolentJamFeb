﻿using UnityEngine;

public class ShipController : MonoBehaviour
{
    private float baseMaxHealth = 20.0f;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            health = Mathf.Clamp(value, 0f, maxHealth);
            //OnStatsChange(health, maxHealth, PowerUpType.Health);
        }
    }
    private bool criticalCondition;

    [SerializeField]
    private int freezeCharges = 1;
    private int maxFreezeCharges = 5;
    public int FreezeCharges
    {
        get { return freezeCharges; }
        set { freezeCharges = Mathf.Clamp(value, 0, maxFreezeCharges); }
    }

    private float baseFreeseDuration = 0.5f;

    [SerializeField]
    private float startFreezeDuration = 2.0f;

    [SerializeField]
    private float freezeDuration = 2.0f;
    private float maxFreezeDuration = 10f;
    public float FreezeDuration
    {
        get { return freezeDuration; }
        set { if (freezeDuration + value < maxFreezeDuration) { freezeDuration = value; } }
    }
    private float baseSpeed = 25;

    [SerializeField]
    private float speed = 50.0f;
    public float Speed
    {
        set { speed = value; }
        get { return speed; }
    }

    private float maxSpeed = 300f;
    private const float baseRateOfFire = 0.5f;

    [SerializeField]
    private float startRateOfFire = 2f;

    [SerializeField]
    private float rateOfFire = 2f;

    [SerializeField]
    private float maxRateOfFire = 10f;

    public float RateOfFire
    {
        get { return rateOfFire; }
        set { rateOfFire = Mathf.Clamp(value, startRateOfFire, maxRateOfFire); }
    }
    private const int baseDamage = 1;
    [SerializeField]
    private uint damage;
    public uint Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    private int maxDamage = 60;

    [SerializeField]
    private GameObject[] blurSprites;

    [SerializeField]
    private GameObject shipCamera;

    private bool isDead = false;
    public static event System.Action OnPlayerDeath;
    public static event System.Action<int> OnFreezeChargeUsed;
    public static event System.Action<bool> OnShipCritical;
    //public static event System.Action<float,float, PowerUpType> OnStatsChange;
    public static event System.Action<int> OnMultiplierChanged;

    void Start()
    {
        Health = maxHealth;
        //UpdateUi with default values
        OnFreezeChargeUsed(freezeCharges);
        //InitialiseUIStat();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < blurSprites.Length; i++)
        {
            blurSprites[i].transform.localPosition = -GetComponent<Rigidbody2D>().velocity / 20f * i;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles") && !isDead)
        {
             //Get bullet damage
            float damage = col.GetComponent<BulletController>().Damage;

            //Remove health from player
            Health -= damage;
            //OnStatsChange(Health, maxHealth, PowerUpType.Health);
            if (Health <= 0f)
            {
                isDead = true;
                OnPlayerDeath.Invoke();
            }
            CheckCriticalCondition();
            //Destroy Bullet
            col.gameObject.SetActive(false);

            StartCoroutine(shipCamera.GetComponent<CameraShake>().Shake());
            
        }

        if(col.gameObject.layer == LayerMask.NameToLayer("Powerup"))
        {
            PowerupPickup(col.gameObject.GetComponent<Powerup>().powerUpType);
            Destroy(col.gameObject);
        }

        if(col.gameObject.layer == LayerMask.NameToLayer("Multiplier"))
        {
            OnMultiplierChanged.Invoke(1);
            Destroy(col.gameObject);
        }

    }
   

    void PowerupPickup(PowerUpType powerUpType)
    {
        print(powerUpType.ToString());
        switch (powerUpType)
        {
            case PowerUpType.Health:
                Health += 50;
                break;
            case PowerUpType.Invun:
                Damage += baseDamage;
                //OnStatsChange.Invoke(Damage,maxDamage, powerUpType);
                break;
            case PowerUpType.Damage:
                FreezeDuration += baseFreeseDuration;
                //OnStatsChange.Invoke(FreezeDuration,maxFreezeDuration, powerUpType);
                break;
            case PowerUpType.Explosion:
                break;
            case PowerUpType.Freeze:
                FreezeCharges++;
                OnFreezeChargeUsed(freezeCharges);
                break;
            default:
                break;
        }
    }

    /*void InitialiseUIStat()
    {
        OnStatsChange.Invoke(rateOfFire, maxRateOfFire, PowerUpType.FireRate);
        OnStatsChange.Invoke(Damage, maxDamage, PowerUpType.TurretDamage);
        OnStatsChange.Invoke(FreezeDuration, maxFreezeDuration, PowerUpType.FreezeTime);
        OnStatsChange.Invoke(freezeCharges, maxFreezeCharges, PowerUpType.FreezeCharge);
        OnStatsChange.Invoke(Speed, maxSpeed, PowerUpType.MoveSpeed);
        OnStatsChange.Invoke(Health, maxHealth, PowerUpType.MaxHealth);
    }*/


    void CheckCriticalCondition()
    {
        if (health < maxHealth * 0.2f)
        {
            criticalCondition = true;
            OnShipCritical.Invoke(criticalCondition);
        }
        else if(criticalCondition = true && health > maxHealth * 0.2f)
        {
            criticalCondition = false;
            OnShipCritical.Invoke(criticalCondition);
        }

       

    }

    public void UseFreezeCharge()
    {
        if(freezeCharges >0)
        { 
            FreezeCharges--;
            OnFreezeChargeUsed.Invoke(freezeCharges);
        }
        
    }

}
