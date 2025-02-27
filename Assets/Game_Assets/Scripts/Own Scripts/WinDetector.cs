using System;
using UnityEngine;

public class WinDetector : MonoBehaviour
{
    public static event Action PlayerWinEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            PlayerWinEvent?.Invoke();
        }
    }
}
