using UnityEngine.SceneManagement;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    [SerializeField] private Collider2D finalCollider;
    [SerializeField] private string _exitSceneName = "Level_2";
    [SerializeField] private string _enterSceneName = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnsceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnsceneLoaded;
    }

    private void OnsceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _enterSceneName = SceneManagement.Instance.TransitionFromCurrentSceneName;

        if (_exitSceneName == _enterSceneName)
        {
            finalCollider.enabled = false;
        }
    }
}
