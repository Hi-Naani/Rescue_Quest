using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerInputControl playerInputControl;
    private bool attackButtonDown, isAttacking = false;
    private float timeBetweenAttacks;

    protected override void Awake()
    {
        base.Awake();
        playerInputControl = new PlayerInputControl();
    }

    private void Start()
    {
        playerInputControl.Combat.Attack.started += _ => StartAttacking(); // for continuous Combat
        playerInputControl.Combat.Attack.canceled += _ => StopAttacking(); // stopping tthe attack

        AttackCoolDown();
    }
    private void OnEnable()
    {
        playerInputControl.Enable();
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Update()
    {
        Attack(); 
    }

    public void AssigningNewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCoolDown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCoolDown;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void AttackCoolDown()
    {
        isAttacking = true;
        StopCoroutine(TimeBetweenAttacksRoutine());
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCoolDown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
        
    }
}
