using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string currentSceneName;
    [SerializeField] private string sceneName;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float waitTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetSceneTransitionName(sceneTransitionName);
            SceneManagement.Instance.SetCurrentSceneName(currentSceneName);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(SetSceneLoadRoutine());
        }
    }

    private IEnumerator SetSceneLoadRoutine()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }
}
