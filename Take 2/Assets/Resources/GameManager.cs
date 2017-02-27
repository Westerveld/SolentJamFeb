using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class GameManager : MonoBehaviour {

    int score;

    //Enemy/Wake managment
    int waveNumber = 0;
    int waveSize;
    public const int initalWaveSize = 10;
    public const int waveSizeIncrement = 5;
    float nextWaveDelaySeconds = 10.0f;
 

    //List enemies
    List<Transform> enemyList = new List<Transform>();

    //<A>collection of enemies
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateWave()
    {

    }

    void NextWave()
    {
        waveNumber++;
        waveSize = 10 + waveNumber * waveSizeIncrement;
        for (int i = 0; i < waveSize; waveSize++ )
        {

        }
    }

    //If Ship Dead 
    void EndGame()
    {
        
    }

}
