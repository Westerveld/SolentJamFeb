using UnityEngine;
using System.Collections;

public class GunController : ComponentController {
    [SerializeField]
    private Transform[] bulletSpawn;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float nextShot;

    [SerializeField]
    private float shotInterval;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private bool currentTurret;

    protected override void Activate()
    {
        if (joystick != "")
        {
            if (Input.GetAxis("Activate" + joystick) > 0)
            {
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + shotInterval;
                    Vector3 direction = ship.transform.position - transform.position;
                    direction.Normalize();

                    GameObject newBullet = (GameObject)Instantiate(bullet);
                    newBullet.transform.rotation = bulletSpawn[currentTurret ? 0 : 1].rotation;
                    newBullet.transform.position = bulletSpawn[currentTurret ? 0 : 1].position;
                    newBullet.GetComponent<Rigidbody2D>().velocity = -direction * bulletSpeed;
                    newBullet.GetComponent<BulletController>().Damage = ship.GetComponent<ShipController>().Damage;
                    gameObject.GetComponent<Animator>().SetTrigger("Shot");
                    currentTurret = !currentTurret;
                }

            }
        }
    }
}
