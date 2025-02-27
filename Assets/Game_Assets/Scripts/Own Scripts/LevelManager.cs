using System;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager>
{
    private HashSet<LevelName> levels = new HashSet<LevelName>();

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeadEvent += ResetLevels;
        WinDetector.PlayerWinEvent += ResetLevels;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeadEvent -= ResetLevels;
        WinDetector.PlayerWinEvent -= ResetLevels;
    }

    private void ResetLevels()
    {
        levels.Clear();
    }

    public void MarkLevelCompleted(LevelName levelName)
    {
        levels.Add(levelName);
        foreach (var levels in levels)
        {
            Console.WriteLine(levels);
        }
    }

    public bool IsLevelCLeared(LevelName levelName)
    {
        bool result = levels.Contains(levelName);
        return result;
    }
}
