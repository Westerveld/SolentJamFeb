using UnityEngine;
using System.Collections;

public class ShieldController : ComponentController {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles"))
        {
            col.gameObject.SetActive(false);
        }
    }
	
}
