using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBurst;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private int projectilesPerBrust;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;

    private bool isShooting = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if(oscillate)
        {
            stagger = true;
        }

        if(!oscillate) { stagger = false; }
        if(projectilesPerBrust < 1) { projectilesPerBrust = 1; }
        if(burstCount < 1) { burstCount = 1; }
        if(timeBetweenBurst < 0.1f) {  timeBetweenBurst = 0.1f; }
        if(restTime < 0.1f) { restTime = 0.1f; }
        if(startingDistance < 0.1f) { startingDistance = 0.1f; }
        if(angleSpread == 0) { projectilesPerBrust = 1; }
        if(bulletMoveSpeed <= 0) { bulletMoveSpeed = 0.1f; }
    }

    public void Attack()
    {
        if (this.transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        if (stagger)
        {
            timeBetweenProjectiles = timeBetweenBurst / projectilesPerBrust;
        }

        for (int i = 0; i < burstCount; i++)
        {
            
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
           
            

            for (int j = 0; j < projectilesPerBrust; j++)
            {

                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                //newBullet.transform.right = -targetDirection;
                newBullet.transform.right = -(newBullet.transform.position - this.transform.position);

                ///<summary> // different ways to update bulletspeed
                /// newBullet.GetComponent<Projectile>().UpdateMoveSpeed(bulletMoveSpeed);
                ///</summary>

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if(stagger) 
                { 
                    yield return new WaitForSeconds(timeBetweenProjectiles); 
                }

            }

             currentAngle = startAngle;

              if (!stagger)
              {
                    yield return new WaitForSeconds(timeBetweenBurst);
              }

        }


        yield return new WaitForSeconds(restTime);
        isShooting = false;

    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - this.transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpeed = 0f;
        angleStep = 0f;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBrust - 1);
            halfAngleSpeed = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpeed;
            endAngle = targetAngle + halfAngleSpeed;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);

        return pos;
    }
}
