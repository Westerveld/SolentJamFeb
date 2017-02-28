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
    float musicTime;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        UpdateMusic(MusicState.MainMenu);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A)) UpdateMusic(MusicState.MainMenu);

        if (Input.GetKeyDown(KeyCode.S)) UpdateMusic(MusicState.InGame);

        if (Input.GetKeyDown(KeyCode.D)) UpdateMusic(MusicState.InGameIntense);


    }

   public void UpdateMusic(MusicState ms)
    {
        switch (ms)
        {
            case MusicState.MainMenu:
                Base.mute = false;
                wave.mute = true;
                intense.mute = true;
                break;
            case MusicState.InGame:
                Base.mute = false;
                wave.mute = false;
                intense.mute = true;
                break;
            case MusicState.InGameIntense:
                Base.mute = false;
                wave.mute = false;
                intense.mute = false;
                break;
            default:
                break;
        }
    }


}
