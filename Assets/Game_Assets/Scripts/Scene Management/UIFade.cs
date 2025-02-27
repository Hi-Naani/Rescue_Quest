using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    // IEnumerator variable can hold the reference of IEnumerator Coroutine
    private IEnumerator fadeRoutine;

    public void FadeToBlack()
    {
        if(fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1); // assigning Ienumarator coroutine to Ienumarator variable
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        StartCoroutine(FadeRoutine(0));
    }
    private IEnumerator FadeRoutine(float targetAmount)
    {
        while (!Mathf.Approximately(fadeScreen.color.a, targetAmount))
        {
            float alpha = Mathf.MoveTowards(fadeScreen.color.a, targetAmount, fadeSpeed * Time.deltaTime);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, alpha);
            yield return null;
        }
    }
}
