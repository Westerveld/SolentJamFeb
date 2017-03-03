using UnityEngine;

public class FreezeController : ComponentController
{
    public static System.Action<float> OnFreeze;

    [SerializeField]
    private Animator animationController;
    private float freezeTime;
    private float freezeDelay = 0.5f;
	protected override void Activate()
    {
        if (Joystick > 0)
        {
            if (Input.GetButtonDown("Activate" + Joystick)&& shipController.FreezeCharges > 0 && Time.time > freezeTime)
            {
                animationController.SetTrigger("Frozen");
                freezeTime = Time.time + freezeDelay;
                OnFreeze(shipController.FreezeDuration);
                shipController.UseFreezeCharge();
            }
        }
    }
}
