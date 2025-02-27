using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    /// <summary>
    /// INcomplete class
    /// </summary>
    private PlayerController playerController;
    private PlayerInputControl playerInputControl;

    private float dashSpeed = 15f;
    private bool isDashing = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerInputControl = new PlayerInputControl();
        playerInputControl.Combat.Dash.performed += ctx => Dash(ctx.ReadValue<float>());
    }

    private void OnEnable()
    {
        playerInputControl.Enable();
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
    }

    public float Dash(float moveSpeed)
    {
        if (!isDashing)
        {
            moveSpeed *= dashSpeed;
            StartCoroutine(StoppingDashRoutine());
        }
        
        return moveSpeed;      
    }

    private IEnumerator StoppingDashRoutine()
    {
        isDashing = true;

        yield return new WaitForSeconds(0.5f);

        
    }
}
