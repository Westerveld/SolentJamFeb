using UnityEngine;
using System.Collections;

public class EngineController : ComponentController {

    [SerializeField]
    protected float force;

    [SerializeField]
    protected Animator flame;

    [SerializeField]
    protected AudioSource audioSource;
    protected override void Activate()
    {
        if (Joystick > 0)
        {
            if (Input.GetButton("Activate" + Joystick))
            {
                Rigidbody2D rigidbody = ship.GetComponent<Rigidbody2D>();
                rigidbody.AddForce((ship.transform.position - transform.position).normalized * force * Time.fixedDeltaTime, ForceMode2D.Force);
                if (rigidbody.velocity.magnitude > ship.GetComponent<ShipController>().Speed)
                {
                    rigidbody.velocity = rigidbody.velocity.normalized * ship.GetComponent<ShipController>().Speed;
                }
                audioSource.Play();
                flame.SetBool("Thrusting", true);
            }
            else
            {
                DisableFlame();
            }
        }
    }

    public void DisableFlame()
    {
        flame.SetBool("Thrusting", false);
        audioSource.Stop();
    }
}
