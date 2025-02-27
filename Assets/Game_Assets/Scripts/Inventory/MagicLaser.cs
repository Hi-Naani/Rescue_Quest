using System.Collections;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 0.22f;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    private float laserRange;
    private bool isGrowing = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    public void GetLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Indestructible>() && !collision.isTrigger)
        {
            isGrowing = false;
        }
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0F;

        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float _linearT = timePassed / laserGrowTime;

            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, _linearT), 1f);

            capsuleCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, _linearT), capsuleCollider.size.y);
            capsuleCollider.offset = new Vector2(Mathf.Lerp(1f, laserRange, _linearT) / 2, capsuleCollider.offset.y);

            yield return null;
        }

        StartCoroutine(gameObject.GetComponent<SpriteFade>().SlowFadeRoutine());
    }

    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = transform.position - mousePos;
        transform.right = -direction;
        
    }
}
