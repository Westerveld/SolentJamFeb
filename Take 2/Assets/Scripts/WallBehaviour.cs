using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour {

    [SerializeField]
    private GameObject whiteExplosion;

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles") || col.gameObject.layer == LayerMask.NameToLayer("Player Projectiles"))
        {
            GameObject explosion = Instantiate(whiteExplosion);
            explosion.transform.position = col.gameObject.transform.position;
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
            col.gameObject.SetActive(false);
        }
    }
}
