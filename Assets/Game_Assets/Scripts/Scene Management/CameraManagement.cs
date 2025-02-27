using System;
using System.Collections;
using Cinemachine;
using UnityEngine.SceneManagement;


public class CameraManagement : Singleton<CameraManagement>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Start()
    {
        SetCinemachineVirtualCamera();       
    }

    public void SetCinemachineVirtualCamera()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode1)
    {
        StartCoroutine(WaitForPlayerAndSetCamera());
    }

    public IEnumerator WaitForPlayerAndSetCamera()
    {
        while(cinemachineVirtualCamera == null)
        {
            cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            yield return null;
        }

        while (PlayerController.Instance == null)
        {
            yield return null;
        }

        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }

}
