using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Kamikaze,
    Normal,
    Advanced
}

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private GameObject ship;
    public GameObject Ship
    {
        get { return ship; }
        set { ship = value; }
    }

    [SerializeField]
    private uint damage;
    public uint Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private int score = 10;
    public int Score
    {
        get { return score; }
        set { score = value; }
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
    private int minMultipliers;
    [SerializeField]
    private int maxMultipliers;



    [SerializeField]
    private float distanceToShoot;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float nextShot;

    [SerializeField]
    private float shotInterval;

    [SerializeField]
    private float multipliersToSpawn;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawn;

    [SerializeField]
    private float bulletSpeed;
    private bool frozen;

    private bool dead;

    private bool deathAnimation; //Used to stop the coroutine for being called consistantly
    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }

    private BulletPool bp;
    public BulletPool Bp
    {
        get { return Bp; }
        set { bp = value; }
    }

    public bool Frozen
    {
        get { return frozen; }
        set { frozen = value; }
    }

    [SerializeField]
    private Animator animationController;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private float dropRate = 5.0f;
    [SerializeField]
    private GameObject powerupPrefab;

    [SerializeField]
    private GameObject multiplierPrefab;

    [SerializeField]
    private float variation = 0.05f;

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
            if(deathAnimation)
            {
                deathAnimation = false;
                StartCoroutine(Die());
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

            // If the player tries to escape, fly really fast
            if (distance > distanceToShoot * 2f)
            {
                float speed = Mathf.Max(ship.GetComponent<Rigidbody2D>().velocity.magnitude * 2f, moveSpeed * 2f);
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
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
            if (distance < distanceToShoot) //Enemy only shoots if they are within the distance to shoot
            {
                nextShot = Time.time + shotInterval;
                bp.ActivateBullet(bulletSpawn.position, bulletSpeed, direction, damage);
                animationController.SetTrigger("shot");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.gameObject.layer == LayerMask.NameToLayer("Player Projectiles"))
        {
             //ToDo:
            //Enemy Colliding with PlayerProjectile
            //Return to object pool
            if (col.gameObject.GetComponent<BulletController>().Damage < health)
            {
                health -= col.gameObject.GetComponent<BulletController>().Damage;
            }
            else
            {
                Death();
            }
            col.gameObject.SetActive(false);
            //Add score to game manager

            //Return enemy to pool

            SpawnExplosion();
        }
        if(col.gameObject.tag == "Asteroid")
        {
            //ToDo:
            //Enemy Colliding With Asteroid
            //Return to object pool
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            Death();
            SpawnExplosion();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player Ships"))
        {
            if (collision.relativeVelocity.magnitude > 4f)
            {
                collision.gameObject.GetComponent<ShipController>().Health = collision.gameObject.GetComponent<ShipController>().Health - health;

                Death();

                SpawnExplosion();
            }
        }
    }

    void Death()
    {
        health = 0;
        dead = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        deathAnimation = true;
    }

    void SpawnExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
    }
        

   public void RandomValues() //Used to variate the ships to have different values
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
        multipliersToSpawn = Random.Range(minMultipliers, maxMultipliers);
    }

    IEnumerator Die()
    {
        OnEnemyDeath(score);
        dead = true;
        animationController.SetTrigger("died");
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(.8f);
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        gameObject.SetActive(false);
        float dropChance = Random.Range(0.0f, 100.0f); //Used to determine whether the enemy drops a powerup
        if (dropChance < dropRate)
        {
            GameObject powerup = (GameObject)Instantiate(powerupPrefab,transform.position, Quaternion.identity);
            powerup.GetComponent<Attract>().Ship = Ship;
        }
        for(int i = 0; i < multipliersToSpawn; i++)
        {
            //Add some variation to the spawn location of the multiplier
            float xPos = Random.Range(transform.position.x - variation, transform.position.x + variation);
            float yPos = Random.Range(transform.position.y - variation, transform.position.y + variation);
            GameObject mp = (GameObject)Instantiate(multiplierPrefab, new Vector3(xPos,yPos, transform.position.z), Quaternion.identity);
            mp.GetComponent<Attract>().Ship = Ship;
        }
    }
}
