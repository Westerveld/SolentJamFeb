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

    [SerializeField]
    private float lifeTime;

    private float startTime;

    [SerializeField]
    private Animator anim;

    private void Start()
    {
        if(this.gameObject.layer == LayerMask.NameToLayer("Powerup") || this.gameObject.layer == LayerMask.NameToLayer("Multiplier"))
        {
            startTime = Time.time;
            Destroy(this.gameObject, lifeTime);
        }
    }

    private void FixedUpdate()
    {
        MoveTowards();

        if (this.gameObject.layer == LayerMask.NameToLayer("Powerup") || this.gameObject.layer == LayerMask.NameToLayer("Multiplier"))
        {
            if (Time.time - startTime > (lifeTime / 2))
            {
                anim.SetBool("Fade", true);
            }
        }
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
