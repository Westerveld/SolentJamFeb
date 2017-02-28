using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

    [SerializeField]
    private float health;
    public float Health
    {
        set { health = value; }
        get { return health; }
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyProjectile")
        {
            //Get bullet damage
            float damage = col.gameObject.GetComponent<BulletController>().Damage;
            //Remove health from player
            health -= damage;
            //Destroy Bullet
        }
    }

}
