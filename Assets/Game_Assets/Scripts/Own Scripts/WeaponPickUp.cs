using System;
using System.Collections.Generic;
using UnityEngine;


public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private List<Transform> weaponPickUpPoints;
    [SerializeField] private GameObject weaponToBeSpawned;
    [SerializeField] private float timer = 0;
    [SerializeField] private LevelName levelName;

    private WeaponType weaponInstantiated = WeaponType.None;
    
    public static Action SwordPickEvent;

    private void Update()
    {
        if(weaponInstantiated != WeaponType.None)
        {
            return;
        }
        else if(timer > 3f && weaponInstantiated == WeaponType.None)
        {
            WeaponInstantiate();
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }

    private void OnEnable()
    {
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Sword] += EquipeSword;
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Staff] += EquipeStaff;
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Bow] += EquipeBow;

        PlayerHealth.OnPlayerDeadEvent += ResetWeaponState;
        PlayerHealth.OnPlayerDeadEvent += UnEquipeWeaponOnPlayerDeath;

        MainUIManager.OnRestart_MainMenuButtonEvent += ResetWeaponState;
        MainUIManager.OnRestart_MainMenuButtonEvent += UnEquipeWeaponOnPlayerDeath;

        WinDetector.PlayerWinEvent += ResetWeaponState;
        WinDetector.PlayerWinEvent += UnEquipeWeaponOnPlayerDeath;
    }

    private void OnDisable()
    {
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Sword] -= EquipeSword;
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Staff] -= EquipeStaff;
        PlayerWeaponPickInfo.weaponPickedEvents[WeaponType.Bow] -= EquipeBow;

        PlayerHealth.OnPlayerDeadEvent -= ResetWeaponState;
        PlayerHealth.OnPlayerDeadEvent -= UnEquipeWeaponOnPlayerDeath;

        MainUIManager.OnRestart_MainMenuButtonEvent -= ResetWeaponState;
        MainUIManager.OnRestart_MainMenuButtonEvent -= UnEquipeWeaponOnPlayerDeath;

        WinDetector.PlayerWinEvent -= ResetWeaponState;
        WinDetector.PlayerWinEvent -= UnEquipeWeaponOnPlayerDeath;
    }

    private void ResetWeaponState()
    {
        weaponInstantiated = WeaponType.None;
    }

    private void WeaponInstantiate()
    {
        if(LevelManager.Instance.IsLevelCLeared(levelName)) { return; }

        int swapnPoint = UnityEngine.Random.Range(0, weaponPickUpPoints.Count);
        GameObject _weapon = Instantiate(weaponToBeSpawned, weaponPickUpPoints[swapnPoint].position, Quaternion.identity);

        if(levelName == LevelName.TownScene)
        {
            weaponInstantiated = WeaponType.Sword;
        }
        else if (levelName == LevelName.Level1)
        {
            weaponInstantiated = WeaponType.Staff;
        }
        else if (levelName == LevelName.Level2)
        {
            weaponInstantiated = WeaponType.Bow;
        }
    }

    private void EquipeWeapon(int slotIndex)
    {
        ActiveInventory.Instance.transform.GetChild(slotIndex).GetComponent<InventorySlot>().SetWeapon();
    }

    private void UnEquipeWeaponOnPlayerDeath()
    {
        int inventorySlots = ActiveWeapon.Instance.transform.childCount;

        for(int i = 0; i <= inventorySlots; i++)
        {
            ActiveInventory.Instance.transform.GetChild(i).GetComponent<InventorySlot>().RemoveWeapon();
        }
    }


    private void EquipeSword() => EquipeWeapon(0);

    private void EquipeBow() => EquipeWeapon(2);

    private void EquipeStaff() => EquipeWeapon(1);

}
