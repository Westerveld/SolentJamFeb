using UnityEngine;
using System;
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
    int spawnRangeMin = 10;
    int spawnWaveMax = 100;

    float nextWaveTime;
    float nextWaveDelaySeconds = 10.0f;
    float spawnOffsetDistance;

    public GameObject Ship;
    public GameObject Enemy;
    //List enemies
    List<GameObject> enemyList = new List<GameObject>();

    public static event Action EnemyDestoyed;
    public static event Action ShipDestoyed;

    //<A>collection of enemies
    // Use this for initialization
    void Start () {
        EnemyDestoyed += enemyDestroyed;
        ShipDestoyed += shipDestroyed;
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
            enemyList.Add(Enemy);
        }

        foreach (GameObject enemy in enemyList)
        {
            Vector3 spawOffset = new Vector3(UnityEngine.Random.Range(spawnRangeMin,spawnWaveMax), UnityEngine.Random.Range(spawnRangeMin, spawnWaveMax), 0);
            enemy.transform.position = Ship.transform.position + spawOffset;
            enemy.SetActive(true);
        }
        enemyCount = enemyList.Count;
        currentGameState = GameState.InWave;

    }

    //If Ship Dead 
    void EndGame()
    {
        
    }

}
