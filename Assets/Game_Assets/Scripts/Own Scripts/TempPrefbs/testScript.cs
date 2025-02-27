using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown) // Detects ANY key press
        {
            Debug.Log("Key Pressed: " + Input.inputString);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape Key Pressed in Main Menu");
        }
    }
}
