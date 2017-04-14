using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class EngineController : ComponentController {

    [SerializeField]
    protected float force;

    [SerializeField]
    protected Animator flame;

    [SerializeField]
    protected AudioSource audioSource;

    
    protected override void Activate()
    {
        if (Joystick != PlayerIndex.Four)
        {
            if (currentControllerState.Triggers.Right != 0.0f)
            {
                Rigidbody2D rigidbody = ship.GetComponent<Rigidbody2D>();
                rigidbody.AddForce((ship.transform.position - transform.position).normalized * force * Time.fixedDeltaTime, ForceMode2D.Force);
                if (rigidbody.velocity.magnitude > ship.GetComponent<ShipController>().Speed)
                {
                    rigidbody.velocity = rigidbody.velocity.normalized * ship.GetComponent<ShipController>().Speed;
                }

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                GamePad.SetVibration(Joystick, 0.2f, 0.0f);
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
        GamePad.SetVibration(Joystick, 0.0f, 0.0f);
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
