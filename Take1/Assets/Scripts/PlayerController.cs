using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float turnSpeed;

    void Update()
    {
        transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime);
    }
}
