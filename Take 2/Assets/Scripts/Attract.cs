using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Attract : MonoBehaviour {

    [SerializeField]
    private GameObject ship;
    public GameObject Ship
    {
        get { return ship; }
        set { ship = value; }
    }

    [SerializeField]
    private Rigidbody2D rigid;

    [SerializeField]
    private float minDistance;

    [SerializeField]
    private float attractForce;

    private void FixedUpdate()
    {
        MoveTowards();
    }
    
    private void MoveTowards()
    {
        Vector2 difference = ship.transform.position - transform.position;
        if(difference.magnitude <= minDistance)
        {
            rigid.AddForce(difference * attractForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
