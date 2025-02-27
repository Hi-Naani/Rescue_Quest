using UnityEngine;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName {  get; private set; }
    public string TransitionFromCurrentSceneName {  get; private set; }

    private void OnEnable()
    {
        WinDetector.PlayerWinEvent += ResetSceneTransitions;
    }

    private void ResetSceneTransitions()
    {
        SceneTransitionName = null;
        TransitionFromCurrentSceneName = null;
    }

    public void SetSceneTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }

    public void SetCurrentSceneName(string currentSceneName)
    {
        this.TransitionFromCurrentSceneName = currentSceneName;
    }
}
