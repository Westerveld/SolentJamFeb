using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    void Update()
    {
        Vector3 parentPosition = transform.parent.position;
        transform.localPosition = new Vector3(-parentPosition.x % 20, -parentPosition.y % 20, -parentPosition.z % 20);
    }
}
