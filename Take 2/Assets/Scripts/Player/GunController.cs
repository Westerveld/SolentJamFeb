using UnityEngine;
using System.Collections;

public class GunController : ComponentController {
    [SerializeField]
    private Transform[] bulletSpawn;

    [SerializeField]
    private PlayerBulletPool playerBulletPool;

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
        if (joystick > 0)
        {
            if (Input.GetAxis("Activate" + joystick) > 0)
            {
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + shotInterval;
                    Vector2 direction = transform.position - ship.transform.position;
                    direction.Normalize();
                    Vector3 shipVelocity = Vector3.Project(ship.GetComponent<Rigidbody2D>().velocity, direction);

                    playerBulletPool.ActivateBullet(bulletSpawn[currentTurret ? 0 : 1].position, bulletSpeed + shipVelocity.magnitude, direction, ship.GetComponent<ShipController>().Damage);
                    gameObject.GetComponent<Animator>().SetTrigger("Shot");
                    currentTurret = !currentTurret;
                }

            }
        }
    }
}
