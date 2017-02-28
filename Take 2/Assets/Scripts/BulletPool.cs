using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {

    [SerializeField]
    private int SpawnCount = 200;

    public GameObject bulletPrefab;

    List<GameObject> bulletList = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < SpawnCount; i++)
        {
            GameObject go = (GameObject)Instantiate(bulletPrefab);
            bulletList.Add(go);
            go.SetActive(false);
        }
    }


    public void ActivateBullet(Vector2 startPos, float velocity, Vector2 direction)
    {
        foreach (GameObject bullet in bulletList)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = startPos;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * velocity;
                bullet.SetActive(true);
                break;
            }
        }
    }

    public void FreezeBullets()
    {
        foreach(GameObject bullet in bulletList)
        {
            if(bullet.activeSelf)
            {
                bullet.GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }

    public void UnFreezeBullets()
    {
        foreach(GameObject bullet in bulletList)
        {
            if(bullet.activeSelf)
            {
                bullet.GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }

}
