using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpwanPoint;
    [SerializeField] private float waitTimeForArrowPrefabToInstantiate = 0.05f;

    private Animator animator;
    readonly int fire_Hash = Animator.StringToHash("SetFire");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Attack()
    {
        animator.SetTrigger(fire_Hash);
        StartCoroutine(InstantiateArrowPrfabRoutine());
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private IEnumerator InstantiateArrowPrfabRoutine()
    {
        yield return new WaitForSeconds(waitTimeForArrowPrefabToInstantiate);
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpwanPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }
}
