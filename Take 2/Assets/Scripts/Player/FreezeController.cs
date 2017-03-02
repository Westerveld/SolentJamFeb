using UnityEngine;

public class FreezeController : ComponentController
{
    public static System.Action<float> OnFreeze;

    private float freezeTime;
    private float freezeDelay = 0.5f;
	protected override void Activate()
    {
        if (joystick > 0)
        {
            if (Input.GetAxis("Activate" + joystick) > 0 && shipController.FreezeCharges > 0 && Time.time > freezeTime)
            {
                freezeTime = Time.time + freezeDelay;
                OnFreeze(shipController.FreezeDuration);
                shipController.UseFreezeCharge();
            }
        }
    }
}
