using UnityEngine;
using XInputDotNetPure;

public class FreezeController : ComponentController
{
    public static System.Action<float> OnFreeze;

    [SerializeField]
    private Animator animationController;
    private float freezeTime;
    private float freezeDelay = 0.5f;
	protected override void Activate()
    {
        if (Joystick != PlayerIndex.Four)
        {
            if (currentControllerState.Triggers.Right > 0 && shipController.FreezeCharges > 0 && Time.time > freezeTime)
            {
                animationController.SetTrigger("Frozen");
                freezeTime = Time.time + freezeDelay;
                OnFreeze(shipController.FreezeDuration);
                shipController.UseFreezeCharge();
            }
        }
    }
}
