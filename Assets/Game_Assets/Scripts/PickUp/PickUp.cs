using System.Collections;
using UnityEngine;


public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        StaminGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUptype;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float pickUpDistance = 5f;
    [SerializeField] private float accelarationRate = 0.2f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    
    private Vector3 moveDirection;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if(Vector3.Distance(this.transform.position, playerPos) < pickUpDistance)
        {
            moveDirection = (playerPos - this.transform.position).normalized;
            moveSpeed += accelarationRate;
        }
        else
        {
            moveDirection = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            DetectPickupType();
            Destroy(this.gameObject);
        }
    }

    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 _startPoint = this.transform.position;
        float randomX = this.transform.position.x + Random.Range(-2f, 2f);
        float randomY = this.transform.position.y + Random.Range(-1f, 1f);

        Vector2 _endPoint = new Vector2(randomX, randomY);

        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float _linearT = timePassed / popDuration;
            float _heightT = animCurve.Evaluate(_linearT);
            float _height = Mathf.Lerp(0f, heightY, _heightT);

            this.transform.position = Vector2.Lerp(_startPoint, _endPoint, _linearT) + new Vector2(0, _height);

            yield return null;
        }

    }

    private void DetectPickupType()
    {
        switch (pickUptype)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                break;

            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                break;

            case PickUpType.StaminGlobe:
                Stamina.Instance.RefreshStamina();
                break;

            default:
                break;

        }
    }
}
