using UnityEngine;
using System.Collections;

public enum PowerUpType
{
    Health,
    //Invun,
    Damage,
    Explosion,
    Freeze,
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
            case PowerUpType.Health:
                myRenderer.sprite = powerupSprites[0];
                break;
            /*case PowerUpType.Invun:
                myRenderer.sprite = powerupSprites[1];
                break;*/
            case PowerUpType.Damage:
                myRenderer.sprite = powerupSprites[2];
                break;
            case PowerUpType.Explosion:
                myRenderer.sprite = powerupSprites[3];
                break;
            case PowerUpType.Freeze:
                myRenderer.sprite = powerupSprites[4];
                break;
            default:
                break;
        }
    }
}
