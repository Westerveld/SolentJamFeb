using UnityEngine;
using System.Collections;

public class ShieldController : ComponentController {

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Enemy Projectiles"))
        {
            col.gameObject.SetActive(false);
        }
        else if(col.gameObject.GetComponent<EnemyController>().m_enemyType == EnemyType.Kamikaze)
        {
            col.gameObject.SetActive(false);   
        }
    }
	
}
