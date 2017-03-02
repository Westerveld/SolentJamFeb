using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

    private uint damage;
    public uint Damage
    {
        set { damage = value; }
        get { return damage; }
    }

    private Vector3 velocity;
    public Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    private const float maxTime = 2f;
    private float time = 0f;

    private Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!rigidBody.isKinematic)
        {
            time += Time.deltaTime;

            if (time >= maxTime)
            {
                StartCoroutine(FadeOut());
            }
        }
    }

    IEnumerator FadeOut()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        TrailRenderer trail = GetComponent<TrailRenderer>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Color color = renderer.color;
        color.a = 0.2f;
        renderer.color = color;

        while (renderer.color.a > 0f)
        {
            color = renderer.color;
            color.a -= 0.05f;
            renderer.color = color;
            Color trailColor = trail.material.GetColor("_Color");
            trailColor.a = renderer.color.a;
            trail.material.SetColor("_Color", trailColor);

            yield return new WaitForSeconds(0.125f);
        }
        
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        time = 0f;

        GetComponent<BoxCollider2D>().enabled = true;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 1f;
        renderer.color = color;

        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.Clear();
        Color trailColor = trail.material.GetColor("_Color");
        trailColor.a = 1f;
        trail.material.SetColor("_Color", trailColor);
    }
}
