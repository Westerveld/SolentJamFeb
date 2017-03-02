using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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
    public int Score
    {
        get { return score; }
        set { score = value;}
    }
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
    void Start()
    {
        ShipController.OnPlayerDeath += EndGame;
        EnemyController.OnEnemyDeath += OnEnemyDestroyed;
        GameStateChanged += ChangeGameState;
        ship = (GameObject)Instantiate(shipPrefab);
        EndWave();
        Time.timeScale = 1;
        FreezeController.OnFreeze += OnFreeze;
    }

    void OnDestroy()
    {
        ShipController.OnPlayerDeath -= EndGame;
        EnemyController.OnEnemyDeath -= OnEnemyDestroyed;
        GameStateChanged -= ChangeGameState;
        FreezeController.OnFreeze -= OnFreeze;
    }

    void OnEnemyDestroyed()
    {
        enemyCount--;
    }

	// Update is called once per frame
	void Update()
    {
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
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
        Time.timeScale = 0;
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Additive);
    }

    void OnFreeze()
    {
        if (!frozen)
        {
            StartCoroutine(Freeze());
        }
    }

    //Freeze all relevent objects (enemy bullets & ships !player ship).
    IEnumerator Freeze()
    {
        Debug.Log("Thing");
        frozen = true;

        //Freeze Enemy ships.
        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = true;
        }

        //Freeze bullets
        bp.FreezeBullets();

        yield return new WaitForSeconds(2f);
        
        //Unfreeze the enemy ships
        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = false;
        }

        //Unfreeze the bullets
        bp.UnFreezeBullets();

        frozen = false;
    }
}
