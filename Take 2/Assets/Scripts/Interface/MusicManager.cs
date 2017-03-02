using UnityEngine;
using System.Collections;

public enum MusicState
{
    MainMenu,
    InGame,
    InGameIntense
}

public class MusicManager : MonoBehaviour {

    public AudioSource Base;
    public AudioSource wave;
    public AudioSource intense;
    GameState currentGamestate;
	// Use this for initialization
	void Start () {
        GameManager.OnGameStateChanged += UpdateMusic;
        ShipController.OnShipCritical += ShipCritical;
	}
    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= UpdateMusic;
        ShipController.OnShipCritical -= ShipCritical;
    }

        void UpdateMusic(GameState gs)
    {
        currentGamestate = gs;
        switch (gs)
        {
            case GameState.BetweenWaves:
                Base.mute = false;
                wave.mute = true;
                intense.mute = true;
                break;
            case GameState.InWave:
                Base.mute = false;
                wave.mute = false;
                intense.mute = true;
                break;
            case GameState.NextWave:
                Base.mute = false;
                wave.mute = true;
                intense.mute = true;
                break;
            case GameState.Paused:
                Base.mute = false;
                wave.mute = true;
                intense.mute = true;
                break;
           
            case GameState.ShipDestroyed:
                Base.mute = true;
                wave.mute = true;
                intense.mute = true;
                break;
            default:
                break;
            
        }

    }

    void ShipCritical(bool isShipCritical)
    {
        if(isShipCritical)
        {
            Base.mute = false;
            wave.mute = false;
            intense.mute = false;
        }
        else
        {
            UpdateMusic(currentGamestate);
        }
        
    }

    


}
