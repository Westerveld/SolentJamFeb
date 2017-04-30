using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private float offset;

    void Update()
    {
        Vector3 parentPosition = transform.parent.position;
        transform.localPosition = new Vector3(-parentPosition.x % offset, -parentPosition.y % offset, -parentPosition.z % offset);
    }
}
