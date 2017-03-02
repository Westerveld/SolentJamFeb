using UnityEngine;

public class ShipController : MonoBehaviour
{
    private float baseMaxHealth = 20.0f;
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float health;
    public float Health
    {
        set
        {
            health = Mathf.Clamp(value, 0f, maxHealth);
           
        }
        get { return health; }
    }
    private bool criticalCondition;

    [SerializeField]
    private int freezeCharges = 1;
    private int maxFreezeCharges = 5;
    public int FreezeCharges
    {
        set { freezeCharges = Mathf.Clamp(value,0,maxFreezeCharges); }
        get { return freezeCharges; }
    }
    private float baseFreeseDuration = 0.5f;
    [SerializeField]
    private float startFreezeDuration = 2.0f;
    [SerializeField]
    private float freezeDuration = 2.0f;
    public float FreezeDuration
    {
        set { freezeDuration = value; }
        get { return freezeDuration; }
    }
    private float baseSpeed = 0.2f;
    [SerializeField]
    private float maxSpeed;
    public float MaxSpeed
    {
        set { maxSpeed = value; }
        get { return maxSpeed; }
    }

    private const float baseRateOfFire = 0.1f;
    [SerializeField]
    private float rateOfFire = 1.0f;
    private float maxRateOfFire; //slowest fire rate.
    public float RateOfFire
    {
        set { RateOfFire = Mathf.Clamp(value,0.1f, maxRateOfFire); }
        get { return rateOfFire; }
    }
    private const int baseDamage = 1;
    [SerializeField]
    private uint damage;
    public uint Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    [SerializeField]
    private GameObject shipCamera;
    private bool isDead = false;
    public static event System.Action OnPlayerDeath;
    public static event System.Action<float> OnPlayerHit;
    public static event System.Action<int> OnFreezeChargeUsed;
    public static event System.Action<bool> OnShipCritical;

    void Start()
    {
        Health = maxHealth;
        //UpdateUi with default values
        OnPlayerHit.Invoke(health);
        OnFreezeChargeUsed(freezeCharges);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles") && !isDead)
        {
             //Get bullet damage
            float damage = col.GetComponent<BulletController>().Damage;

            //Remove health from player
            Health -= damage;
            if (Health <= 0f)
            {
                isDead = true;
                OnPlayerDeath.Invoke();
            }
            OnPlayerHit.Invoke(Health);
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

    }

    void PowerupPickup(PowerUpType powerUpType)
    {
        print(powerUpType.ToString());
        switch (powerUpType)
        {
            case PowerUpType.FireRate:
                RateOfFire -= baseRateOfFire;
                break;
            case PowerUpType.TurretDamage:
                Damage += baseDamage;
                break;
            case PowerUpType.FreezeTime:
                FreezeDuration += baseFreeseDuration;
                break;
            case PowerUpType.FreezeCharge:
                FreezeCharges++;
                break;
            case PowerUpType.MoveSpeed:
                MaxSpeed += baseSpeed;
                break;
            case PowerUpType.MaxHealth:
                maxHealth += baseMaxHealth;
                break;
            case PowerUpType.Health:
                Health += 50;
                break;
            default:
                break;
        }
    }

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
