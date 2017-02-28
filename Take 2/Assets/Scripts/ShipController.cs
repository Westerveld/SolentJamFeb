using UnityEngine;

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
    private GameObject shipCamera;

    public static event System.Action OnPlayerDeath;

    void Start()
    {
        Health = maxHealth;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy Projectile")
        {
            //Get bullet damage
            float damage = col.GetComponent<BulletController>().Damage;

            //Remove health from player
            Health = Health - damage;

            //Destroy Bullet
            col.gameObject.SetActive(false);

            StartCoroutine(shipCamera.GetComponent<CameraShake>().Shake());
        }
    }
}
