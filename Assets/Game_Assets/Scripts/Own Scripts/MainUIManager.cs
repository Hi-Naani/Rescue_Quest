using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainUIManager : Singleton<MainUIManager>
{
    [SerializeField] GameObject[] MainMenuPanels; 
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject BestScore;
    [SerializeField] GameObject InstructionsPage;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] ScrollRect scrollRect;
    

    [SerializeField] GameTimer gameTimer;
 

    public static event Action OnRestart_MainMenuButtonEvent;

    private UI_Controls ui_Controls;

    const string townSceneName = "Town";
    const string mainmenusceneName = "MainMenu";

    private void Start()
    {
        PausePanel.SetActive(false);
        gameTimer.GetBestScore();
        scrollbar.size = 0.2f;
    }

    private void OnEnable()
    {
        ui_Controls = new UI_Controls();
        ui_Controls.Enable();
        SceneManager.sceneLoaded += ActivateSceneUI;
        ui_Controls.UI_Input.EscapeforPause.started += ExecutePause;
        WinDetector.PlayerWinEvent += DisplayWinScreen;
        PlayerHealth.OnPlayerDeadEvent += DisplayLooseScreen;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ActivateSceneUI;
        WinDetector.PlayerWinEvent -= DisplayWinScreen;
        PlayerHealth.OnPlayerDeadEvent -= DisplayLooseScreen;
    }

    private void DisplayLooseScreen()
    {
        float waitTime = 2f;

        while (waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
        }

        this.transform.GetChild(2).gameObject.SetActive(true);
        SetTimeToZero();

    }

    private void DisplayWinScreen()
    {
        this.transform.GetChild(1).gameObject.SetActive(true);
        SetTimeToZero();
    }

    private void ExecutePause(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && !PausePanel.activeInHierarchy && !InstructionsPage.activeInHierarchy)
        {
            if (MainMenuPanels[1].activeInHierarchy) return;
            if (MainMenuPanels[2].activeInHierarchy) return;

            PausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0 && (BestScore.activeInHierarchy || InstructionsPage.activeInHierarchy))
        {
            BestScore.SetActive(false);
            InstructionsPage.SetActive(false);
            MainMenuPanels[4].SetActive(false);
            MainMenuPanels[0].SetActive(true);
        }
        else if(SceneManager.GetActiveScene().buildIndex != 0 && InstructionsPage.activeInHierarchy)
        {
            InstructionsPage.SetActive(false);
            MainMenuPanels[4].SetActive(false);
            PausePanel.SetActive(true);
            MainMenuPanels[3].SetActive(true);
        }
       
    }


    private void ActivateSceneUI(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            for(int i = 0; i < MainMenuPanels.Length; i++)
            {
                MainMenuPanels[i].SetActive(false);

                if (i == 0)
                {
                    MainMenuPanels[i].SetActive(true);
                }
               
            }

        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    MainMenuPanels[i].SetActive(true);
                }
                else
                {
                    MainMenuPanels[i].SetActive(false);
                }

            }
            
        }
    }

    public void OnPlayButtonPressed()
    {
        UIFade.Instance.FadeToBlack();
        StartCoroutine(LoadSceneRoutine(townSceneName, 1f));
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Quit Application");
        Application.Quit();
    }

    private IEnumerator LoadSceneRoutine(string SceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneName);
    }

    public void OnResumeButtonPressed()
    {
        PausePanel.SetActive(false);
        SetTimeToNormal();
    }

    public void OnRestartButtonPressed()
    {
        OnRestart_MainMenuButtonEvent?.Invoke();
        PausePanel.SetActive(false);
        SetTimeToNormal();
        UIFade.Instance.FadeToBlack();
        StartCoroutine(LoadSceneRoutine(townSceneName, 1f));
    }

    public void OnMainMenuButtonPressed()
    {
        OnRestart_MainMenuButtonEvent?.Invoke();

        MainMenuPanels[1].SetActive(false);
        MainMenuPanels[2].SetActive(false);
        PausePanel.SetActive(false);
        SceneManager.LoadScene(mainmenusceneName);
        SetTimeToNormal();

    }

    private void SetTimeToNormal()
    {
        Time.timeScale = 1f;
    }

    private void SetTimeToZero()
    {
        Time.timeScale = 0f;
    }

    public void OnBestScoreButtonPressed()
    {
        MainMenuPanels[0].SetActive(false);
        MainMenuPanels[4].SetActive(true);
        MainMenuPanels[4].transform.GetChild(1).gameObject.SetActive(true);
        MainMenuPanels[4].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnInsructionsButtonPressed()
    {
        StartCoroutine(ResetScrollbar());

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenuPanels[0].SetActive(false);
        }
        else
        {
            MainMenuPanels[3].SetActive(false);
            PausePanel.SetActive(false);
        }

        MainMenuPanels[4].SetActive(true);
        MainMenuPanels[4].transform.GetChild(0).gameObject.SetActive(true);
        MainMenuPanels[4].transform.GetChild(1).gameObject.SetActive(false);


    }

    public void OnResetBestScoreButtonPressed()
    {
        gameTimer.RestBestScore();
    }

    IEnumerator ResetScrollbar()
    {
        yield return null;
        scrollbar.size = 0.3f;
        scrollbar.value = 1f;
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
