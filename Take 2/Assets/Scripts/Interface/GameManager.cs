using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public enum GameState
{
    BetweenWaves,
    InWave,
    Paused,
    ShipDestroyed,
    NextWave,
    EndGame
}
[System.Serializable]	
public class EnemySpawnObjects
{
	[SerializeField]
	public GameObject prefab;
	[SerializeField]
	public int spawnWeight;
	[SerializeField]
	public int minSpawnwave;
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
    private GameState lastGameState;
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
  //  [SerializeField]

    //private GameObject[] enemyPrefabs;
    public EnemySpawnObjects[] enemySpawnObjects;
    private GameObject ship;
    //List enemies
    private List<GameObject> enemyList = new List<GameObject>();

    //Multiplier
    private int multiplier = 1;
    public int Multiplier
    {
        get { return multiplier; }
        set { multiplier = value; }
    }

    private GamePadState controllerState;


    //Action used to update sound & ui based on the changing gamestate
    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int> OnWaveChanged;
    public static event System.Action OnWaveEnded;

    bool gameOver = false;
    //<A>collection of enemies
    // Use this for initialization
    void Start()
    {
        //controllerStates[0] = GamePad.GetState(PlayerIndex.One);

        /*if(SceneManager.GetActiveScene().name == "PaulTesting")
        {
            controllerStates[1] = GamePad.GetState(PlayerIndex.Two);
        }*/
        ShipController.OnPlayerDeath += EndGame;
        EnemyController.OnEnemyDeath += OnEnemyDestroyed;
        OnGameStateChanged += ChangeGameState;
        ShipController.OnMultiplierChanged += AddToMultiplier;
        ship = (GameObject)Instantiate(shipPrefab);
        EndWave();
        Time.timeScale = 1;
        FreezeController.OnFreeze += OnFreeze;
        PauseMenuController.OnGameResumed += Resume;
       
    }

    void OnDestroy() 
    {
        ShipController.OnPlayerDeath -= EndGame;
        EnemyController.OnEnemyDeath -= OnEnemyDestroyed;
        OnGameStateChanged -= ChangeGameState;
        FreezeController.OnFreeze -= OnFreeze;
        PauseMenuController.OnGameResumed -= Resume;
    }

    void OnEnemyDestroyed(int value)
    {
        score += (value * multiplier);
        enemyCount--;
        OnScoreChanged.Invoke(score);
    }

	// Update is called once per frame
	void Update()
    {
        if (!gameOver)
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
                case GameState.EndGame:
                    break;
                default:
                    break;
            }
        }
        CheckControllers();
    }

    void SetPause()
    {
        if (currentGameState != GameState.ShipDestroyed && currentGameState != GameState.EndGame)
        {
            if (currentGameState == GameState.Paused)
            {
                //Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        lastGameState = currentGameState;
        OnGameStateChanged(GameState.Paused);
    }

    void Resume()
    {
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseMenu");
        OnGameStateChanged(lastGameState);
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
        OnWaveEnded.Invoke();
        OnGameStateChanged(GameState.BetweenWaves);
    }

    void NextWave()
    {
		print ("next wave waveNumber = " + waveNumber);
        int startCount = 0;
        waveNumber++;
        OnWaveChanged.Invoke(waveNumber);
        waveSize = initalWaveSize + waveNumber * waveSizeIncrement;
        if (enemyList.Count < waveSize)
        {
            startCount = enemyList.Count;
        }

        for (int i = startCount; i < waveSize; i++)
        {
			//Initialise new enemy object when the need is larger than the current pool size.
			int totalWeight = 0;
			//Calculate total weight for chance to add enemy type.
			foreach (EnemySpawnObjects eo in enemySpawnObjects)
			{
				if (eo.minSpawnwave >= waveNumber) 
				{
					totalWeight += eo.spawnWeight;
				}
			}
			//Calculate enemy type to add to list. 
			int rand = Random.Range(0, totalWeight);
			int currentWeightCount = 0;
			int previousWeightCount = 0;
			GameObject go;
			for (int j = 0; j< enemySpawnObjects.Length; j++)
			{
				if (enemySpawnObjects[j].minSpawnwave <= waveNumber) 
				{
					currentWeightCount += enemySpawnObjects[j].spawnWeight;
					if (rand >= previousWeightCount && rand < currentWeightCount) 
					{
						go = Instantiate(enemySpawnObjects[j].prefab);
						EnemyController ec = go.GetComponent<EnemyController>();
						ec.Ship = ship;
						ec.Bp = bp;
						go.transform.parent = gameObject.transform;
						//Add to enemy list.
						enemyList.Add(go);
					}
					previousWeightCount = currentWeightCount;
				}
			}
        
        }


        foreach (GameObject enemy in enemyList)
        {
            enemy.transform.position = ship.transform.position + GetRandomSpawnPosition();
            enemy.SetActive(true);
            enemy.GetComponent<EnemyController>().Dead = false;
            enemy.GetComponent<EnemyController>().RandomValues();
            //Enemy values are reset when destroyed.
        }
        enemyCount = enemyList.Count;
        OnGameStateChanged(GameState.InWave);

    }

    Vector3 GetRandomSpawnPosition()
    {
        //Get random value between the give distances
        int x = Random.Range(waveSpawnDistanceFromPlayerMin, waveSpawnDistanceFromPlayerMax);
        int y = Random.Range(waveSpawnDistanceFromPlayerMin, waveSpawnDistanceFromPlayerMax);
        //Flip a coin and invert the x value.
        if (Random.Range(0, 2) == 1)
        {
            x = -x;
        }
        //Flip a coin and invert the y value.
        if (Random.Range(0, 2) == 1)
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
        gameOver = true;
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.Save();
        Time.timeScale = 0;
        SceneManager.LoadScene("EndScreen", LoadSceneMode.Additive);
        OnGameStateChanged(GameState.ShipDestroyed);
    }

    void OnFreeze(float freezeDuration)
    {
        if (!frozen)
        {
            StartCoroutine(Freeze(freezeDuration));
        }
    }

    //Freeze all relevent objects (enemy bullets & ships !player ship).
    IEnumerator Freeze(float freezeDuration)
    {
        frozen = true;

        //Freeze Enemy ships.
        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = true;
        }

        //Freeze bullets
        bp.FreezeBullets();

        yield return new WaitForSeconds(freezeDuration);
        
        //Unfreeze the enemy ships
        foreach (GameObject go in enemyList)
        {
            go.GetComponent<EnemyController>().Frozen = false;
        }

        //Unfreeze the bullets
        bp.UnFreezeBullets();

        frozen = false;
    }

    private void AddToMultiplier(int value)
    {
        Multiplier += value;
    }

    private void CheckControllers()
    {
        for (int i = 0; i < 2; i++)
        {
            controllerState = GamePad.GetState((PlayerIndex)i);
            if (controllerState.Buttons.Start == ButtonState.Pressed)
            {
                SetPause();
            }
        }
    }
}
