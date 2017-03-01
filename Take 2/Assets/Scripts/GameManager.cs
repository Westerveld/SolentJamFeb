﻿using UnityEngine;
//using System;
using System.Collections.Generic;

public enum GameState
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
    private int waveSpawnDistanceFromPlayerMin = 10;
    [SerializeField]
    private int waveSpawnDistanceFromPlayerMax = 30;


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

    //Action used to update sound & ui based on the changing gamestate
    public static event System.Action<GameState> GameStateChanged;

    //<A>collection of enemies
    // Use this for initialization
    void Start () {
        ShipController.OnPlayerDeath += EndGame;
        EnemyController.OnEnemyDeath += EnemyDestroyed;
        GameStateChanged += ChangeGameState;
        ship = (GameObject)Instantiate(shipPrefab);
        EndWave();
    }

    void EnemyDestroyed()
    {
        enemyCount--;
    }

    void shipDestroyed()
    {
        GameStateChanged(GameState.ShipDestroyed);
    }
	

	// Update is called once per frame
	void Update () {
  

        //Check if things are frozen and if they need to be unfrozen based on freeze time.
        if (frozen && Time.time >= freezeTime)
        {
            UnFreeze();
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
        GameStateChanged(GameState.BetweenWaves);
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
         
        }

        foreach (GameObject enemy in enemyList)
        {
             enemy.transform.position = ship.transform.position + GetRandomSpawnPosition();
             enemy.SetActive(true);
            //Enemy values are reset when destroyed.
        }
        enemyCount = enemyList.Count;
        GameStateChanged(GameState.InWave);

    }
    Vector3 GetRandomSpawnPosition()
    {
        //Get random value between the give distances
        int x = Random.Range(waveSpawnDistanceFromPlayerMin, waveSpawnDistanceFromPlayerMax);
        int y = Random.Range(waveSpawnDistanceFromPlayerMin, waveSpawnDistanceFromPlayerMax);
        //Flip a coin and invert the x value.
        if (Random.Range(0, 1) == 1)
        {
            x = -x;
        }
        //Flip a coin and invert the y value.
        if (Random.Range(0, 1) == 1)
        {
            y = -y;
        }

        return new Vector3(x, y, 0);
    }
    private void ChangeGameState(GameState gameState)
    {
        currentGameState = gameState;
    }

    //If Ship Dead 
    void EndGame()
    {
        Debug.Log("End The Game");
    }

    //Freeze all relevent objects (enemy bullets & ships !player ship).
   public void Freeze()
    {
        freezeTime = Time.time + ship.GetComponent<ShipController>().Health;
        frozen = true;

        //Freeze Enemy ships.
        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = true;
        }

        //Freeze bullets
        bp.FreezeBullets();
    }

    //Unfreezes all frozen objects.
    void UnFreeze()
    {
            //Unfreeze the enemy ships
            foreach (GameObject go in enemyList)
            {
                go.GetComponent<EnemyController>().Frozen = false;
            }
            //Unfreeze the bullets
            bp.UnFreezeBullets();
    }

}
