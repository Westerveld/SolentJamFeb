using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour
{
    [SerializeField]
    private GameObject engine;

    [SerializeField]
    private float rotationSpeed;

    void Update()
    {
        engine.transform.RotateAround(transform.position, Vector3.back, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
    }
}
