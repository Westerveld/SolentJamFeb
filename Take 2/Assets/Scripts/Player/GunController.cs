using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class GunController : ComponentController {
    [SerializeField]
    private Transform[] bulletSpawn;

    [SerializeField]
    private PlayerBulletPool playerBulletPool;

    [SerializeField]
    private float nextShot;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private bool currentTurret;

    [SerializeField]
    protected AudioSource audioSource;

    protected override void Activate()
    {
        if (Joystick != PlayerIndex.Four)
        {
            if (currentControllerState.Triggers.Right > 0)
            { 
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + (1f / shipController.RateOfFire);
                    Vector2 direction = transform.position - ship.transform.position;
                    direction.Normalize();
                    Vector3 shipVelocity = Vector3.Project(ship.GetComponent<Rigidbody2D>().velocity, direction);

                    playerBulletPool.ActivateBullet(bulletSpawn[currentTurret ? 0 : 1].position, bulletSpeed + shipVelocity.magnitude, direction, shipController.Damage);
                    gameObject.GetComponent<Animator>().SetTrigger("Shot");
                    currentTurret = !currentTurret;

                    //Trying out the audio for the bullets in this script, as the sound cuts off if the player hits a enemy with the bullet
                    audioSource.Play();
                }

            }
        }
    }
}
