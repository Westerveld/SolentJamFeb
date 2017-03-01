using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    [SerializeField]
    private GameObject engine;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float force;
    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private string joystick;

    void FixedUpdate()
    {
        engine.transform.RotateAround(transform.position, Vector3.back, Input.GetAxis("Horizontal" + joystick) * rotationSpeed * Time.fixedDeltaTime);

        if (Input.GetButton("Activate" + joystick))
        {
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.AddForce((transform.position - engine.transform.position).normalized * force * Time.fixedDeltaTime, ForceMode2D.Force);

            if (rigidBody.velocity.magnitude > maxSpeed)
            {
                rigidBody.velocity = rigidBody.velocity.normalized * maxSpeed;
            }
        }
    }
}
