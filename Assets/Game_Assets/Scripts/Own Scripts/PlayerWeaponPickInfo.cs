using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponPickInfo : MonoBehaviour
{
    public static Dictionary<WeaponType, Action> weaponPickedEvents = new Dictionary<WeaponType, Action>() 
    {   
        {WeaponType.Sword, null },
        {WeaponType.Staff, null },
        {WeaponType.Bow, null }   
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        WeaponType weaponType = WeaponType.None;
         
        if (other.gameObject.CompareTag("SwordImage"))
        {
            weaponType = WeaponType.Sword;
        }
        else if (other.gameObject.CompareTag("StaffImage"))
        {
            weaponType = WeaponType.Staff;
        }
        else if (other.gameObject.CompareTag("BowImage"))
        {
            weaponType = WeaponType.Bow;
        }

        if(weaponType != WeaponType.None)
        {
            WeaponPickedEventAssign(weaponType, other.gameObject);
        }

    }

    private void WeaponPickedEventAssign(WeaponType weaponType, GameObject weaponObject)
    {
        weaponPickedEvents[weaponType]?.Invoke();
        Destroy(weaponObject);
    }

}
