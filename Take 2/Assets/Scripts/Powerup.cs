using UnityEngine;
using System.Collections;

public enum PowerUpType
{
    FireRate,
    TurretDamage,
    FreezeTime,
    FreezeCharge,
    MoveSpeed,
    MaxHealth,
    Health,
    Size
}

public class Powerup : MonoBehaviour {

    public PowerUpType powerUpType;

    void Start()
    {
        
       powerUpType = (PowerUpType)Random.Range(0, (int)PowerUpType.Size);
    }
}
