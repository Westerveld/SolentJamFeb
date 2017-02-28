﻿using UnityEngine;
//using System;
using System.Collections.Generic;

enum GameState
{
    BetweenWaves,
    InWave,
    Paused,
    ShipDestroyed,
    NextWave
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BulletPool bp;
    [SerializeField]
    private int score;
    private GameState currentGameState;
    //Enemy/Wake managmeny
    private int waveNumber = 0;
    private int enemyCount;
    private int waveSize;
    [SerializeField]
    private const int initalWaveSize = 10;
    [SerializeField]
    private const int waveSizeIncrement = 5;
    [SerializeField]
    private const float timeBetweenWavesSeconds = 10.0f;
    [SerializeField]
    private int spawnRangeMin = -30;
    [SerializeField]
    private int spawnWaveMax = 30;

    private float nextWaveTime;
    [SerializeField]
    private float nextWaveDelaySeconds = 10.0f;
    private float spawnOffsetDistance;

    //Freeze
    private float freezeTime;
    private bool frozen = false;
    [SerializeField]
    private GameObject shipPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    private GameObject ship;
    //List enemies
    private List<GameObject> enemyList = new List<GameObject>();
    //Sound
    [SerializeField]
    private MusicManager musicManager;

    //<A>collection of enemies
    // Use this for initialization
    void Start () {
        ShipController.OnPlayerDeath += EndGame;
        ship = (GameObject)Instantiate(shipPrefab);
        EndWave();


    }

    void enemyDestroyed()
    {
        enemyCount--;
    }

    void shipDestroyed()
    {
       currentGameState = GameState.ShipDestroyed;
    }
	

	// Update is called once per frame
	void Update () {
        //Debug key space = Freeze
        if (Input.GetKeyDown(KeyCode.Space)) Freeze(2.0f);
               
        if(frozen && Time.time >= freezeTime)
        { 
            foreach (GameObject go in enemyList)
            {
                go.GetComponent<EnemyController>().Frozen = false;
            }

            bp.UnFreezeBullets();
        }


        switch (currentGameState)
        {
            case GameState.BetweenWaves:
                if (Time.time > nextWaveTime)
                {
                    currentGameState = GameState.NextWave;
                }
                break;
            case GameState.InWave:
                    UpdateWave();
                if(ship.GetComponent<ShipController>().Health<50) musicManager.UpdateMusic(MusicState.InGameIntense);
                break;
            case GameState.Paused:
                break;
            case GameState.ShipDestroyed:
                EndGame();
                break;
            case GameState.NextWave:
                NextWave();
                break;
            default:
                break;
        }

	}

    void UpdateWave()
    {
        if(enemyCount <= 0)
        {
            EndWave();
          
        }
        else
        {
            foreach (GameObject ec in enemyList)
            {
                ec.GetComponent<EnemyController>().EnemyFunctions();
            }
        }

    }

    void EndWave()
    {
        nextWaveTime = Time.time + timeBetweenWavesSeconds;
        currentGameState = GameState.BetweenWaves;
        musicManager.UpdateMusic(MusicState.MainMenu);
    }

    void NextWave()
    {
        int startCount = 0;
        waveNumber++;
        waveSize = initalWaveSize + waveNumber * waveSizeIncrement;
        if (enemyList.Count < waveSize)
        {
            startCount = enemyList.Count;
        }
       
        for (int i = startCount; i < waveSize; i++ )
        {
          GameObject go =  (GameObject)Instantiate(enemyPrefab);
            EnemyController ec = go.GetComponent<EnemyController>();
            ec.Ship = ship;
            ec.Bp = bp;
            go.transform.parent = gameObject.transform;
            enemyList.Add(go);
            musicManager.UpdateMusic(MusicState.InGame);
        }

        foreach (GameObject enemy in enemyList)
        {
             enemy.transform.position = ship.transform.position + new Vector3(Random.Range(spawnRangeMin, spawnWaveMax), Random.Range(spawnRangeMin, spawnWaveMax),0);
             enemy.SetActive(true);
            //Reset Enemy values - health, position, etc
        }
        enemyCount = enemyList.Count;
        currentGameState = GameState.InWave;

    }
    //If Ship Dead 
    void EndGame()
    {
        Debug.Log("End The Game");
    }


   public void Freeze(float timeToFreeze)
    {
        Debug.Log("Frozen | GameManager");
        freezeTime = Time.time + timeToFreeze;
        frozen = true;

        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = true;
        }

        bp.FreezeBullets();

        //Call bulete freeze...!!! < < < > > > 
    }

}
