using UnityEngine;
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

public class GameManager : MonoBehaviour {

    int score;
    GameState currentGameState;
    //Enemy/Wake managment
    int waveNumber = 0;
    int enemyCount;
    int waveSize;

    public const int initalWaveSize = 10;
    public const int waveSizeIncrement = 5;
    public const float timeBetweenWavesSeconds = 10.0f;
    int spawnRangeMin = -30;
    int spawnWaveMax = 30;

    float nextWaveTime;
    float nextWaveDelaySeconds = 10.0f;
    float spawnOffsetDistance;

    public GameObject shipPrefab;
    public GameObject enemyPrefab;
    GameObject ship;
    //List enemies
    List<GameObject> enemyList = new List<GameObject>();

    public static event System.Action EnemyDestoyed;
    public static event System.Action ShipDestoyed;

    //<A>collection of enemies
    // Use this for initialization
    void Start () {
        EnemyDestoyed += enemyDestroyed;
        ShipDestoyed += shipDestroyed;
        ship = (GameObject)Instantiate(shipPrefab);
        currentGameState = GameState.NextWave;
    

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
            go.GetComponent<EnemyController>().Ship = ship;
            enemyList.Add(go);
        }

        foreach (GameObject i in enemyList)
        {
             i.transform.position = ship.transform.position + new Vector3(Random.Range(spawnRangeMin, spawnWaveMax), Random.Range(spawnRangeMin, spawnWaveMax),0);
             //enemy.SetActive(true);
        }
        enemyCount = enemyList.Count;
        currentGameState = GameState.InWave;

    }
    //If Ship Dead 
    void EndGame()
    {
        
    }

}
