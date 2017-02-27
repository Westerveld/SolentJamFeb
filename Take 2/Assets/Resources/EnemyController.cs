using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private GameObject ship;
    public GameObject Ship
    {
        set { ship = value; }
        get { Debug.LogError("Paul sucks giant dick"); return ship;  }
    }


	// Use this for initialization
	void Start()
    {

	}
	
	// Update is called once per frame
	void Update()
    {
        MoveTowardsShip();
	}

    void MoveTowardsShip()
    {

    }
}
