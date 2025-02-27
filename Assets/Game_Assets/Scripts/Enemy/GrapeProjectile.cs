using System.Collections;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private GameObject grapeProjectileShadowPrefab;
    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;

    private void Start()
    {
        GameObject _grapeShadow = Instantiate(grapeProjectileShadowPrefab, this.transform.position + new Vector3(0f, -0.3f, 0f), Quaternion.identity);

        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = _grapeShadow.transform.position;

        StartCoroutine(ProjectileCurvedRoutine(this.transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(_grapeShadow, grapeShadowStartPosition, playerPos));
    }

    private IEnumerator ProjectileCurvedRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f ,heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2 (0f, height);

            yield return null;
        }

        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while(timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;

            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);

            yield return null;
        }

        Destroy(grapeShadow);
    }
};