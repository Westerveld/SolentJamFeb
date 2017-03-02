using UnityEngine;
using System.Collections.Generic;

public class PlayerBulletPool : MonoBehaviour {

    [SerializeField]
    private int SpawnCount = 200;

    public GameObject bulletPrefab;

    List<GameObject> bulletList = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            GameObject go = (GameObject)Instantiate(bulletPrefab);
            go.transform.parent = this.gameObject.transform;
            bulletList.Add(go);
            go.SetActive(false);
        }
    }

    //Start position
    public void ActivateBullet(Vector2 startPos, float velocity, Vector2 direction, uint damage)
    {
        foreach (GameObject bullet in bulletList)
        {
            if (!bullet.activeSelf)
            {
                bullet.SetActive(true);
                bullet.transform.position = startPos;
                bullet.GetComponent<Rigidbody2D>().isKinematic = false;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * velocity;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
                bullet.GetComponent<BulletController>().Damage = damage;

                break;
            }
        }
    }
}
