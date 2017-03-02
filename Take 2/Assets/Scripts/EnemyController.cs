using UnityEngine;
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
    private uint damage;
    public uint Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    private int score = 10;
    public int Score
    {
        set { score = value; }
        get { return score; }
    }

    private const uint maxHealth = 10;
    [SerializeField]
    private uint health = maxHealth;

    [SerializeField]
    private float distanceToShip;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float rotationSpeedMin;
    [SerializeField]
    private float rotationSpeedMax;
    [SerializeField]
    private float distanceMin;
    [SerializeField]
    private float distanceMax;
    [SerializeField]
    private float moveSpeedMin;
    [SerializeField]
    private float moveSpeedMax;
    [SerializeField]
    private float shotIntervalMin;
    [SerializeField]
    private float shotIntervalMax;

    [SerializeField]
    private float distanceToShoot;

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

    private bool dead;
    public bool Dead
    {
        set { dead = value; }
        get { return dead; }
    }

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

    public static event System.Action<int> OnEnemyDeath;
    public void EnemyFunctions()
    {
        if(!frozen)
        {
            if (!dead)
            {
                Move();
                Rotate();
                Shoot();
            }
        }
    }
    
    void Move()
    {
        float distance = Vector2.Distance(ship.transform.position, transform.position);

        if (distance > distanceToShip)
        {
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

            float distance = Vector2.Distance(ship.transform.position, transform.position);
            if (distance > distanceToShoot)
            {
                nextShot = Time.time + shotInterval;
                bp.ActivateBullet(bulletSpawn.position, bulletSpeed, direction, damage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("Hit by " + col.gameObject.name);
        if(col.gameObject.layer == LayerMask.NameToLayer("Player Projectiles"))
        {
            print("Hit by Bullet");
            //ToDo:
            //Enemy Colliding with PlayerProjectile
            //Return to object pool
            if (col.gameObject.GetComponent<BulletController>().Damage <= health)
            {
                health -= col.gameObject.GetComponent<BulletController>().Damage;
            }
            else
            {
                health = 0;
                OnEnemyDeath(score);
                gameObject.SetActive(false);
                dead = true;
            }
            col.gameObject.SetActive(false);
            //Add score to game manager

            //Return enemy to pool

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
    

   public void RandomValues()
    {
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        if (Random.Range(0, 2) == 1)
        {
            rotationSpeed = -rotationSpeed;
        }
        shotInterval = Random.Range(shotIntervalMin, shotIntervalMax);
        distanceToShip = Random.Range(distanceMin, distanceMax);
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
        distanceToShoot = distanceToShip * 2f;
    }

}
