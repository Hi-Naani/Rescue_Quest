using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private Collider2D[] portalExitCollider;
    [SerializeField] private EnemySpawner enemySpawner;
    


    private LevelName levelName;
    private bool portalEnabled = false;
    [SerializeField] private int enemyDeadCount = 0;
    private int maxEnemy; 

    private void Start()
    {
        levelName = enemySpawner.LevelName;
        maxEnemy = enemySpawner.MaxEnemiesPerLevel;

        if(LevelManager.Instance.IsLevelCLeared(levelName))
        {
            foreach(Collider2D col in portalExitCollider)
            {
                col.enabled = true;
            }

            portalEnabled = true;
            enemySpawner.gameObject.SetActive(false);
        }
        else
        {
            foreach (Collider2D col in portalExitCollider)
            {
                col.enabled = false;
            }
            
        }
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeadEvent += ResetEnablePortal;
        WinDetector.PlayerWinEvent += ResetEnablePortal;
        enemySpawner.OnEnemyDead += ActivatePortal;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeadEvent -= ResetEnablePortal;
        WinDetector.PlayerWinEvent -= ResetEnablePortal;
        enemySpawner.OnEnemyDead -= ActivatePortal;
    }

    private void ActivatePortal()
    {
        if (portalEnabled) return;

        enemyDeadCount++;

        if(enemyDeadCount >= maxEnemy)
        {
            foreach (Collider2D col in portalExitCollider)
            {
                col.enabled = true;
            }

            LevelManager.Instance.MarkLevelCompleted(levelName);
            portalEnabled = true;

        } 
    }

    private void ResetEnablePortal()
    {
        portalEnabled = false;
        enemyDeadCount = 0;
    }
}
