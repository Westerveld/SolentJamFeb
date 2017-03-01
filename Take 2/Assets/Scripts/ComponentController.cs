using UnityEngine;
using System.Collections;

public class ComponentController : MonoBehaviour {
    
    [SerializeField]
    protected GameObject ship;

    [SerializeField]
    protected float rotationSpeed;

    [SerializeField]
    protected string joystick;

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Rotate();
        Activate();
	}

    protected virtual void Rotate()
    {
        //Rotates the object around a fixed point, in this instance it rotates around the ships pivot point
        transform.RotateAround(ship.transform.position, Vector3.back, Input.GetAxis("Horizontal" + joystick) * rotationSpeed * Time.fixedDeltaTime);

    }

    protected virtual void Activate()
    {
        
    }
}
