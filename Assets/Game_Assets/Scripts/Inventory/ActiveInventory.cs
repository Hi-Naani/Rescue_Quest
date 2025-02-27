using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private PlayerInputControl playerInputControl;
    private int activeSlotIndexNum = 0;

    protected override void Awake()
    {
        base.Awake();
        playerInputControl = new PlayerInputControl();
    }

    private void Start()
    {
        playerInputControl.Inventory.WeaponInput.performed += ctx => ToogleActiveSlot((int)ctx.ReadValue<float>());

    }

    private void OnEnable()
    {
        playerInputControl.Enable();
        PlayerHealth.OnPlayerDeadEvent += ResetWeaponHighlight;
        WinDetector.PlayerWinEvent += ResetWeaponHighlight;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeadEvent -= ResetWeaponHighlight;
        WinDetector.PlayerWinEvent -= ResetWeaponHighlight;
    }

    private void ResetWeaponHighlight()
    {
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToogleActiveSlot(int inputValue)
    {
        ToggleActiveHighlight(inputValue - 1);
    }

    private void ToggleActiveHighlight(int inputnum)
    {
        activeSlotIndexNum = inputnum;

        foreach(Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        Debug.Log(activeSlotIndexNum.ToString());
        this.transform.GetChild(activeSlotIndexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if(PlayerHealth.Instance.IsDead) { return; }

        if(ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null || !inventorySlot.IsWeaponEquipped)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpwan = weaponInfo.weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpwan, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f); 

        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.AssigningNewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
