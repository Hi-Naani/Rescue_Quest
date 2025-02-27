using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;
    private KnockBack knockBack;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (knockBack.GettingKnocked)
        {
            return;
        }

        rb.MovePosition(rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime));

        if(moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0) 
        {
            spriteRenderer.flipX = false;
        }

    }

    public void MoveTo(Vector2 targetPosition)
    {
        moveDirection = (targetPosition - (Vector2)transform.position).normalized;
    }

    public void StopMoving()
    {
        moveDirection = Vector3.zero;
    }
}
