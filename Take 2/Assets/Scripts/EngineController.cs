using UnityEngine;
using System.Collections;

public class EngineController : ComponentController {

    [SerializeField]
    protected float force;

    protected override void Activate()
    {
        if(Input.GetButton("Activate" + joystick))
        {
            Rigidbody2D rigidbody = ship.GetComponent<Rigidbody2D>();
            float engineAngle = transform.rotation.eulerAngles.z;
            rigidbody.AddForce((ship.transform.position - transform.position).normalized * force * Time.fixedDeltaTime, ForceMode2D.Force);
            if(rigidbody.velocity.magnitude > ship.GetComponent<ShipController>().MaxSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * ship.GetComponent<ShipController>().MaxSpeed;
            }
        }
    }
}
