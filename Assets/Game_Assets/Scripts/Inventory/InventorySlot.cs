using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;

    private bool isWeaponEquipped;
    public bool IsWeaponEquipped { get { return isWeaponEquipped; } private set { } }
    

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void SetWeapon()
    {
        isWeaponEquipped = true;
        this.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void RemoveWeapon()
    {
        isWeaponEquipped = false;
        this.transform.GetChild(1).gameObject.SetActive(false);
    }

}   
