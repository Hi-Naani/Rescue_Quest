using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Tilemap tilemap;
    [SerializeField]private float fadeTime = 0.4f;
    [Range(0, 1)]
    [SerializeField] private float fadeAmount = 0.97f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
               
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer)
            {
                StartCoroutine(FadingRoutine(spriteRenderer, spriteRenderer.color.a, fadeAmount, fadeTime));
            }
            else if(tilemap)
            {
                StartCoroutine(FadingRoutine(tilemap, tilemap.color.a, fadeAmount, fadeTime));
            }
            
        }
               
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer)
            {
                StartCoroutine(FadingRoutine(spriteRenderer, fadeAmount, 1f, fadeTime));
            }
            else if (tilemap)
            {
                StartCoroutine(FadingRoutine(tilemap, fadeAmount, 1f, fadeTime));
            }
        }
        
    }

    private IEnumerator FadingRoutine(SpriteRenderer spriteRenderer, float startValue, float targerValue, float fadeTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float _newAplhaValue = Mathf.Lerp(startValue, targerValue, elapsedTime/fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.b, spriteRenderer.color.g, _newAplhaValue);
            yield return null;
        }

    }

    private IEnumerator FadingRoutine(Tilemap tilemap, float startValue, float targerValue, float fadeTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float _newAplhaValue = Mathf.Lerp(startValue, targerValue, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.b, tilemap.color.g, _newAplhaValue);
            yield return null;
        }

    }
}
