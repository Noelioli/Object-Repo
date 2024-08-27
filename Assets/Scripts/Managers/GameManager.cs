using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPositions; // Easy: 8, Medium: 14, Hard: 20 

    [Header("Game Variables")]
    private float enemySpawnRate; // Now set by script
    [SerializeField] private GameObject playerPrefab;

    public Action OnGameStart;
    public Action OnGameOver;

    // Difficulty varaibles
    int difficultyLevel;
    public int BossHealth;

    // Spawning Enemies
    private GameObject tempEnemy;
    private bool isEnemySpawing;
    public bool isPlaying;
    float bossCount = 25f; //Timer to spawning boss
    float bossIncrement = 25f; // Base gapbetween bosses
    bool spawnBoss;

    // Powerups
    public int nukes;
    public bool machineGunActive;
    public float fillPercentage;
    private float totalTime;
    private float currentTime;

    //  References
    private Player player;
    public ScoreManager scoreManager;
    public UIManager uiManager;
    public PickupSpawner pickupSpawner;

    /// <summary>
    /// Singleton
    /// </summary>
    private static GameManager instance;

    public static GameManager GetInstance()
    {
        return instance;
    }

    void SetSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Awake()
    {
        SetSingleton();
    }
    // ----------------------------

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        UpdateTimer();
        DifficultySlider();
        BossSpawn();
    }

    public void StartGame()
    {
        player = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity).GetComponent<Player>();
        player.OnDeath += StopGame;
        isPlaying = true;

        OnGameStart?.Invoke();
        StartCoroutine(GameStarter());
    }

    IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(2f);
        isEnemySpawing = true;
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {
        while (isEnemySpawing)
        {
            //Before timer
            yield return new WaitForSeconds(1.0f / enemySpawnRate);
            //After timer
            if (isPlaying)
                CreateEnemy();
        }
    }

    void CreateEnemy()
    {
        int indexNumber = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        var chosenPrefab = enemyPrefabs[indexNumber];
        tempEnemy = Instantiate(chosenPrefab);
        tempEnemy.transform.position = spawnPositions[UnityEngine.Random.Range(0, difficultyLevel)].position;
        switch (indexNumber) //Hard coded in the order Melee, Exploder, Shooter, Machinegun, because GetEnemyType was only returning Melee.
        {
            case 0:
                tempEnemy.GetComponent<Melee>().SetMeleeEnemy(0.75f, 0.25f);
                break;
            case 1:
                break;
            case 2:
                tempEnemy.GetComponent<Shooter>().SetShooterEnemy(3.5f, 3f);
                tempEnemy.GetComponent<Shooter>().weapon = new Weapon("Shooter Weapon", 40f, 20f);
                break;
            case 3:
                tempEnemy.GetComponent<Machinegun>().SetMachinegunEnemy(2.5f, 0.2f);
                tempEnemy.GetComponent<Machinegun>().weapon = new Weapon("Machinegun Weapon", 2f, 10f);
                break;
        }
        if (spawnBoss)
        {
            tempEnemy.AddComponent<Boss>();
            spawnBoss = false;
        }
    }

    public void StopGame()
    {
        isEnemySpawing = false;
        isPlaying = false;
        scoreManager.SetHighScore();
        StartCoroutine(GameStopper());
    }

    IEnumerator GameStopper()
    {
        isEnemySpawing = false;
        yield return new WaitForSeconds(2f);
        isPlaying = false;

        foreach(Enemy item in FindObjectsOfType(typeof(Enemy)))
        {
            item.Die();
        }
        foreach(Pickup item in FindObjectsOfType(typeof(Pickup)))
        {
            Destroy(item.gameObject);
        }

        OnGameOver?.Invoke();
    }

    public void NotifyDeath(Enemy enemy)
    {
        pickupSpawner.SpawnPickup(enemy.transform.position);
    }

    public void FindPlayer()
    {
        try
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        catch(NullReferenceException e)
        {
            Debug.Log("No player in scene");
        }
    }

    public Player GetPlayer() { return player; }

    public void AddNuke()
    {
        // Increases nuke count and updates nuke icons
        nukes++;
        uiManager.UpdatePowerups();
    }

    public void RemoveNuke()
    {
        // Decreases nuke count and updates nuke icons
        nukes--;
        uiManager.UpdatePowerups();
    }

    public void Countdown(float time)
    {
        // Starts machine gun timer and resets the amount if another powerup is picked up
        totalTime = time;
        currentTime = time;
        uiManager.countdown.gameObject.SetActive(true);
    }

    private void UpdateTimer()
    {
        //A simple timer function that divides the two states
        if (uiManager.countdown.isActiveAndEnabled && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            fillPercentage = currentTime / totalTime;
            uiManager.UpdateCountdown();
            machineGunActive = true;
        }
        else
        {
            uiManager.countdown.gameObject.SetActive(false);
            machineGunActive = false;
        }
    }

    void DifficultySlider()
    {
        int tempscore = scoreManager.GetScore();
        if (tempscore > 200)
        {
            pickupSpawner.SetProbability(0.4f);
            difficultyLevel = 20;
            BossHealth = (int)tempscore / 50;
            enemySpawnRate = 2f;
        }
        else if (tempscore > 25)
        {
            pickupSpawner.SetProbability(1f/MathF.Log(tempscore));//curve
            difficultyLevel = 14;
            BossHealth = 3;
            enemySpawnRate = tempscore / 50f;
        } 
        else
        {
            pickupSpawner.SetProbability(0.7f);
            difficultyLevel = 8;
            BossHealth = 3;
            enemySpawnRate = 0.5f;
        }
    }

    void BossSpawn()
    {
        if (scoreManager.GetScore() >= bossCount)
        {
            bossCount += bossIncrement;

            spawnBoss = true;
        }
    }
}
