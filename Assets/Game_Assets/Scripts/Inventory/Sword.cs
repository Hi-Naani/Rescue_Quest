using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{

    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject slashAnimPrefab;

    private Animator animator;
    private GameObject slashAnim;
    private Transform weaponCollider;
    private Transform slashAnimSpawnPoint;
    private int animatorID;

    private void Awake()
    {
        animator = GetComponent<Animator>();  
    }

    private void Start()
    {
        animatorID = Animator.StringToHash("Attack");    

        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        weaponCollider.gameObject.SetActive(false);

        slashAnimSpawnPoint = PlayerController.Instance.GetSlashAnimSpawnPoint();

    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {     
        animator.SetTrigger(animatorID);

        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;           
        
    }

    public void OnAttackCompletion()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if(PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingFlipDownAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void MouseFollowWithOffset()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 mousePosition = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(playerPos);

        float angleZ = 0;  //Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;


        if(mousePosition.x < playerPosition.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angleZ);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angleZ);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

}


