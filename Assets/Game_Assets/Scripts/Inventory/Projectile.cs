using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22F;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = this.transform.position;
    }

    void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    private void MoveProjectile()
    {
        this.transform.Translate(Vector3.left *  moveSpeed * Time.deltaTime);
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.GetComponent<Indestructible>();
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        
        if(!collision.isTrigger && (enemyHealth || indestructible || player))
        {
            if ((player && isEnemyProjectile) || (enemyHealth) && !isEnemyProjectile)
            {
                player?.TakeDamage(1, this.transform);
                GameObject particelVFX = Instantiate(particleOnHitPrefabVFX, this.transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
            else if(!collision.isTrigger && indestructible)
            {
                GameObject particelVFX = Instantiate(particleOnHitPrefabVFX, this.transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        
    }

    private void DetectFireDistance()
    {
        if(Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(this.gameObject);
        }
    }
}
