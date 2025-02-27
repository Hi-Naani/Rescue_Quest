using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamingDirChangeTime = 4f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCoolDown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    

    private bool canAttack = true;
    private enum State
    {
        Roaming,
        Attacking
    }

    private State state;
    private EnemyPathFinding enemyPathFinding;
    private Vector2 roamPosition;
    private float roamTime = 0f;

    private void Awake()
    {
        enemyPathFinding = GetComponent<EnemyPathFinding>();
    }

    private void OnEnable()
    {
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;

            default:
                break;
        }
    }

    private void Roaming()
    {
        roamTime += Time.deltaTime;
        enemyPathFinding.MoveTo(roamPosition);

        if(roamTime > roamingDirChangeTime)
        {
            roamTime = 0f;
            roamPosition = GetRoamingPosition();
        }

        if (PlayerController.Instance != null)
        {
            if(Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) < attackRange)
            {
                state = State.Attacking;
            }
        }
    }

    private void Attacking()
    {
        if (PlayerController.Instance != null)
        {
            if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) > attackRange)
            {
                state = State.Roaming;
            }
        }

        if (stopMovingWhileAttacking)
        {
            enemyPathFinding.StopMoving();
        }
        else
        {
            enemyPathFinding.MoveTo(roamPosition);
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();
            StartCoroutine(AttackCoolDownRoutine());
        }

    }

    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        
        roamTime = 0f;
        Vector2 randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        return (Vector2)transform.position + randomDirection * 2f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Indestructible>() || collision.gameObject.GetComponent<Destructible>())
        {
            roamPosition = GetRoamingPosition();
            roamTime = 0;
            enemyPathFinding.MoveTo(roamPosition);
        }
    }

}
