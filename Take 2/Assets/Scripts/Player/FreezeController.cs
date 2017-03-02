using UnityEngine;

public class FreezeController : ComponentController
{
    public static System.Action<float> OnFreeze;

	protected override void Activate()
    {
        if (joystick > 0)
        {
            if (Input.GetAxis("Activate" + joystick) > 0 && shipController.FreezeCharges > 0)
            {
                OnFreeze(shipController.FreezeDuration);
                shipController.UseFreezeCharge();
            }
        }
    }
}
