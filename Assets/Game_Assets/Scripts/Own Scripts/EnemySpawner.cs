using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private int maxEnemyCount = 6;
    [SerializeField] private int maxEnemiesPerLevel = 15;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float damageInterval = 6f;
    [SerializeField] private int damageAmount = 1;

    [SerializeField] private int currentEnemyCount = 0;
    [SerializeField] private int totalEnemyCount = 0;
    [SerializeField] private float lastSpawnTime = 0f;
    [SerializeField] private float lastDamageTime = 0f;

    [SerializeField] private LevelManager levelManager;

    [SerializeField] private LevelName levelName;
    public LevelName LevelName { get { return levelName; } private set { } }


    public int MaxEnemiesPerLevel { get { return maxEnemiesPerLevel; } private set { } }

    public event Action OnEnemyDead;

    private IObjectPool<GameObject> enemyPool;
    private float currentTime;



    private void Awake()
    {
        enemyPool = new ObjectPool<GameObject>(CreateEnemy, EnemyOnGet, EnemyOnRelease);
    }

    private void Start()
    {
        currentTime = 0;

        if(LevelManager.Instance.IsLevelCLeared(LevelName))
        {
            this.enabled = false;
        }

        ResetTimer();
    }

    private void ResetTimer()
    {
        lastSpawnTime = 2f;
        currentTime = 0;
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeadEvent += ResetTotalEnemyCount_Timer;
        MainUIManager.OnRestart_MainMenuButtonEvent += ResetTotalEnemyCount_Timer;
        WinDetector.PlayerWinEvent += ResetTotalEnemyCount_Timer;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeadEvent -= ResetTotalEnemyCount_Timer;
        MainUIManager.OnRestart_MainMenuButtonEvent -= ResetTotalEnemyCount_Timer;
        WinDetector.PlayerWinEvent -= ResetTotalEnemyCount_Timer;
    }

    private void ResetTotalEnemyCount_Timer()
    {
        this.enabled = true;
        totalEnemyCount = 0;
        currentEnemyCount = 0;
        currentTime = 0;

        ResetTimer();

    }

    private void EnemyOnRelease(GameObject enemy)
    {
        enemy.SetActive(false);
        OnEnemyDead?.Invoke();
        currentEnemyCount--;
    }

    private void EnemyOnGet(GameObject enemy)
    {
        enemy.transform.position = enemySpawnPoints[GetRandomPosition()].position;
        enemy.SetActive(true);
        currentEnemyCount++;
        totalEnemyCount++;
    }

    private GameObject CreateEnemy()
    {
        GameObject _enemy = Instantiate(enemyPrefab, enemySpawnPoints[GetRandomPosition()].position, Quaternion.identity);
        _enemy.transform.parent = this.transform;
        _enemy.GetComponent<EnemyHealth>().SetEnemyPool(enemyPool);
        return _enemy;
    }

    private int GetRandomPosition()
    {
        return UnityEngine.Random.Range(0, enemySpawnPoints.Count);
    }

    private void Update()
    {
        if (levelManager.IsLevelCLeared(levelName))
        {
            return;
        }
        
        currentTime = Time.time;

        // Spawning logic
        if (totalEnemyCount < maxEnemiesPerLevel && currentEnemyCount < maxEnemyCount)
        {
            if (currentTime - lastSpawnTime >= spawnInterval)
            {
                lastSpawnTime = currentTime;
                enemyPool.Get();
            }
        }

        // Player damage logic
        if (currentEnemyCount >= 3 && currentTime - lastDamageTime >= damageInterval)
        {
            lastDamageTime = currentTime;
            PlayerHealth.Instance.TakeDamage(damageAmount);
        }
        
    }

}
