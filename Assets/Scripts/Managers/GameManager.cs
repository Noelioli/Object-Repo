using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPositions;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;
    [SerializeField] private GameObject playerPrefab;

    public Action OnGameStart;
    public Action OnGameOver;

    private GameObject tempEnemy;
    private bool isEnemySpawing;
    private bool isPlaying;

    public int nukes;

    private Player player;
    public ScoreManager scoreManager;
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
            CreateEnemy();
        }
    }

    void CreateEnemy()
    {
        int indexNumber = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        var chosenPrefab = enemyPrefabs[indexNumber];
        tempEnemy = Instantiate(chosenPrefab);
        tempEnemy.transform.position = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)].position;
        switch (indexNumber) //Hard coded in the order Melee, Exploder, Shooter, Machinegun, because GetEnemyType was only returning Melee.
        {
            case 0:
                tempEnemy.GetComponent<Melee>().SetMeleeEnemy(1.5f, 0.25f);
                break;
            case 1:
                break;
            case 2:
                tempEnemy.GetComponent<Shooter>().SetShooterEnemy(5f, 3f);
                tempEnemy.GetComponent<Shooter>().weapon = new Weapon("Shooter Weapon", 40f, 20f);
                break;
            case 3:
                tempEnemy.GetComponent<Machinegun>().SetMachinegunEnemy(4f, 0.2f);
                tempEnemy.GetComponent<Machinegun>().weapon = new Weapon("Machinegun Weapon", 2f, 10f);
                break;
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
            Destroy(item.gameObject);
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
}
