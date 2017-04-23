using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class ComponentController : MonoBehaviour
{
    [SerializeField]
    protected GameObject ship;
    protected ShipController shipController;

    [SerializeField]
    protected float rotationSpeed;

    [SerializeField]
    protected PlayerIndex joystick;
    public PlayerIndex Joystick
    {
        get { return joystick; }
        set { joystick = value; }
    }

    protected GamePadState currentControllerState;

    protected GamePad gamePad;

    void Start()
    {
        shipController = ship.GetComponent<ShipController>();
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        currentControllerState = GamePad.GetState(Joystick);
        Rotate();
        Activate();
	}

    protected virtual void Rotate()
    {
        //Rotates the object around a fixed point, in this instance it rotates around the ships pivot point
        if (Joystick != PlayerIndex.Four)
        {
            transform.RotateAround(ship.transform.position, Vector3.back, currentControllerState.ThumbSticks.Left.X * rotationSpeed * Time.fixedDeltaTime);
        }

    }

    protected virtual void Activate()
    {
    }
}
