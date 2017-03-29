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

    [SerializeField]
    private Sprite[] powerupSprites;

    private SpriteRenderer myRenderer;


    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        powerUpType = (PowerUpType)Random.Range(0, (int)PowerUpType.Size);
        SetSprite();
    }

    void SetSprite()
    {
        switch(powerUpType)
        {
            case PowerUpType.FireRate:
                myRenderer.sprite = powerupSprites[0];
                break;
            case PowerUpType.TurretDamage:
                myRenderer.sprite = powerupSprites[1];
                break;
            case PowerUpType.FreezeTime:
                myRenderer.sprite = powerupSprites[2];
                break;
            case PowerUpType.FreezeCharge:
                myRenderer.sprite = powerupSprites[3];
                break;
            case PowerUpType.MoveSpeed:
                myRenderer.sprite = powerupSprites[4];
                break;
            case PowerUpType.MaxHealth:
                myRenderer.sprite = powerupSprites[5];
                break;
            case PowerUpType.Health:
                myRenderer.sprite = powerupSprites[6];
                break;
        }
    }
}
