using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth = 2;
    [SerializeField] private float knockBackThrust = 15;
    [SerializeField] private GameObject slimeDeathVFXPrefab;

    private Flash flash;
    private KnockBack knockBack;
    private int currentHealth;
    private IObjectPool<GameObject> enemypPool;

    private void Start()
    {
        flash = GetComponent<Flash>();  
        knockBack = GetComponent<KnockBack>();
    }

    private void OnEnable()
    {
        currentHealth = initialHealth;      
    }

    public void SetEnemyPool(IObjectPool<GameObject> enemyPool)
    {
        this.enemypPool = enemyPool;
    }

    public void HealthDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GotKnocked(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoredefaultMateTime());
        DetectDeath();
        Debug.Log(currentHealth);
        
    }

    public void DetectDeath()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("I am Dead");
            InstantiationMethod(slimeDeathVFXPrefab);
            //Destroy(this.gameObject);
            enemypPool.Release(this.gameObject);
            GetComponent<PickUpSpawner>().DropItems();
        }
    }

    private void InstantiationMethod(GameObject obj)
    {
        Instantiate(obj, this.transform.position, Quaternion.identity);
    }
}
