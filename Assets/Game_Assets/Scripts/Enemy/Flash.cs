using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material flashMate;
    private Material defaultMate;

    [SerializeField] private float restoredefaultMateTime = 0.1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMate = spriteRenderer.material;
    }

    public float GetRestoredefaultMateTime()
    {
        return restoredefaultMateTime;
    }

    public IEnumerator Flashing()
    {
        spriteRenderer.material = flashMate;
        yield return new WaitForSeconds(restoredefaultMateTime);
        spriteRenderer.material = defaultMate;
        StopCoroutine(Flashing());
    }
}
