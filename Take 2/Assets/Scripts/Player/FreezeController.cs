using UnityEngine;

public class FreezeController : ComponentController
{
    public static System.Action OnFreeze;

	protected override void Activate()
    {
        if (joystick > 0)
        {
            if (Input.GetAxis("Activate" + joystick) > 0)
            {
                OnFreeze();
            }
        }
    }
}
