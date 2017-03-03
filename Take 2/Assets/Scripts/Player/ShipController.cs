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
    private float maxFreezeDuration = 10f;
    public float FreezeDuration
    {
        set { freezeDuration = value; }
        get { return freezeDuration; }
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
        set { rateOfFire = Mathf.Clamp(value, startRateOfFire, maxRateOfFire); }
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
    private int maxDamage = 60;
    [SerializeField]
    private GameObject[] blurSprites;

    [SerializeField]
    private GameObject shipCamera;
    private bool isDead = false;
    public static event System.Action OnPlayerDeath;
    public static event System.Action<int> OnFreezeChargeUsed;
    public static event System.Action<bool> OnShipCritical;
    public static event System.Action<float,float, PowerUpType> OnStatsChange;

    void Start()
    {
        Health = maxHealth;
        //UpdateUi with default values
        OnFreezeChargeUsed(freezeCharges);
        InitialiseUIStat();
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
            OnStatsChange(Health, maxHealth, PowerUpType.Health);
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

    }
   

    void PowerupPickup(PowerUpType powerUpType)
    {
        print(powerUpType.ToString());
        switch (powerUpType)
        {
            case PowerUpType.FireRate:
                RateOfFire = RateOfFire + baseRateOfFire;
                OnStatsChange.Invoke(RateOfFire,maxRateOfFire , powerUpType);
                break;
            case PowerUpType.TurretDamage:
                Damage += baseDamage;
                OnStatsChange.Invoke(Damage,maxDamage, powerUpType);
                break;
            case PowerUpType.FreezeTime:
                FreezeDuration += baseFreeseDuration;
                OnStatsChange.Invoke(FreezeDuration,maxFreezeDuration, powerUpType);
                break;
            case PowerUpType.FreezeCharge:
                FreezeCharges++;
                OnFreezeChargeUsed(freezeCharges);
                break;
            case PowerUpType.MoveSpeed:
                Speed += baseSpeed;
                OnStatsChange.Invoke(Speed,maxSpeed,powerUpType);
                break;
            case PowerUpType.MaxHealth:
                maxHealth += baseMaxHealth;
                OnStatsChange.Invoke(Health, maxHealth, powerUpType);
                break;
            case PowerUpType.Health:
                Health += 50;
                OnStatsChange.Invoke(Health, maxHealth, powerUpType);
                break;
            default:
                break;
        }
    }

    void InitialiseUIStat()
    {
        OnStatsChange.Invoke(rateOfFire, maxRateOfFire, PowerUpType.FireRate);
        OnStatsChange.Invoke(Damage, maxDamage, PowerUpType.TurretDamage);
        OnStatsChange.Invoke(FreezeDuration, maxFreezeDuration, PowerUpType.FreezeTime);
        OnStatsChange.Invoke(freezeCharges, maxFreezeCharges, PowerUpType.FreezeCharge);
        OnStatsChange.Invoke(Speed, maxSpeed, PowerUpType.MoveSpeed);
        OnStatsChange.Invoke(Health, maxHealth, PowerUpType.MaxHealth);
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
