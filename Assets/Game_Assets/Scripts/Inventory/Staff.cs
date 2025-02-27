using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpwanPoint;

    private Animator animator;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
    }

    public void SpwanStaffProjectileAnimEvent()
    {
        GameObject _newLaser = Instantiate(magicLaser, magicLaserSpwanPoint.transform.position, transform.rotation);
        _newLaser.GetComponent<MagicLaser>().GetLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(playerPos);

        float angleZ = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        if (mousePos.x < playerPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angleZ);   
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }

    }
}
