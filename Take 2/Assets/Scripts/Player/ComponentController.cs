using UnityEngine;
using System.Collections;

public class ComponentController : MonoBehaviour
{
    [SerializeField]
    protected GameObject ship;
    protected ShipController shipController;

    [SerializeField]
    protected float rotationSpeed;

    [SerializeField]
    protected int joystick;
    public int Joystick
    {
        set { joystick = value; }
        get { return joystick; }
    }

    void Start()
    {
        shipController = ship.GetComponent<ShipController>();
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        Rotate();
        Activate();
	}

    protected virtual void Rotate()
    {
        //Rotates the object around a fixed point, in this instance it rotates around the ships pivot point
        if (Joystick > 0 )
        {
            transform.RotateAround(ship.transform.position, Vector3.back, Input.GetAxis("Horizontal" + Joystick) * rotationSpeed * Time.fixedDeltaTime);
        }

    }

    protected virtual void Activate()
    {
    }
}
