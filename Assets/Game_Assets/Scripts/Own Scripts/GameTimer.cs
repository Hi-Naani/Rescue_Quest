using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text BestTimerText;
    [SerializeField] TMP_Text WinScreenScore;

    private float timer = 0;
    private bool stopTimer = false;
    private float currentTime;
    private float bestTime = 0;

    public void GetBestScore()
    {
        bestTime = PlayerPrefs.GetFloat("BestScore");
        BestTimerText.text = SetBestTime(bestTime);
    }

    public void RestBestScore()
    {
        PlayerPrefs.SetFloat("BestScore", 0);
        PlayerPrefs.Save();
        bestTime = 0;
        BestTimerText.text = SetBestTime(0);
    }

    private void OnEnable()
    {
        WinDetector.PlayerWinEvent += BestTimeCheck;
        WinDetector.PlayerWinEvent += SetWinScore;
        WinDetector.PlayerWinEvent += StopTimerOnWinOrLoose;
        MainUIManager.OnRestart_MainMenuButtonEvent += ResetTimerToZero;
        PlayerHealth.OnPlayerDeadEvent += StopTimerOnWinOrLoose;
    }

    private void OnDisable()
    {
        WinDetector.PlayerWinEvent -= BestTimeCheck;
        WinDetector.PlayerWinEvent -= SetWinScore;
        WinDetector.PlayerWinEvent -= StopTimerOnWinOrLoose;
        MainUIManager.OnRestart_MainMenuButtonEvent -= ResetTimerToZero;
        PlayerHealth.OnPlayerDeadEvent -= StopTimerOnWinOrLoose;
    }   

    private void SetWinScore()
    {
        float score = TimeMinusGoldCoins(timer);

        WinScreenScore.text = SetBestTime(score);
    }

    private void ResetTimerToZero()
    {
        timer = 0;
        TimerTextUpdate(timer);
        stopTimer = true;
        Invoke("ResetStopTimer", 1f);
    }

    private void ResetStopTimer()
    {
        stopTimer = false;
    }

    private void Update()
    {
        if(!stopTimer)
        {
            timer += Time.deltaTime;
            TimerTextUpdate(timer);
        }
    }

    private void TimerTextUpdate(float timer)
    {
        string convertedTimer = string.Format("{0:00}:{1:00}", timer / 60, timer % 60);
        timerText.text = convertedTimer;
    }

    private void StopTimerOnWinOrLoose()
    {
        stopTimer = true;
    }

    private void BestTimeCheck()
    {
        currentTime = TimeMinusGoldCoins(timer);

        if(bestTime == 0f  || bestTime > currentTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestScore", bestTime);
            PlayerPrefs.Save();

            BestTimerText.text = SetBestTime(bestTime);
        }
    }

    private string SetBestTime(float value)
    {              
        string ConvertedBestTimer = string.Format("{0:00}:{1:00}", value / 60, value % 60);
        return ConvertedBestTimer;       
    }

    private float TimeMinusGoldCoins(float value)
    {
        float goldCount = EconomyManager.Instance.CurrentGold;

        float score = value - (goldCount * 0.25f);

        return score;
    }

}
