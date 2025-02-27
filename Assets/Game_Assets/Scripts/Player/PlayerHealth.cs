using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] readonly int maxHealth = 25;

    [SerializeField] private float damageRecoveryTime = 0.5f;

    private int currentHealth;
    private float knockBackThrust = 10f;
    private bool canTakeDamage = true;
    private Flash flash;
    private KnockBack knockBack;
    private Slider healthSlider;

    public bool IsDead { get; private set; }
    public static Action OnPlayerDeadEvent;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    const string HEALTH_SLIDER_TEXT = "/UICanvas/GamePlayUI/Health Bar Container/Health Slider";

    protected override void Awake()
    {
        base.Awake();
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;
        UpdateHealthSlider();
    }

    private void OnEnable()
    {
        MainUIManager.OnRestart_MainMenuButtonEvent += ResetPlayerHealth;
    }

    private void OnDisable()
    {
        MainUIManager.OnRestart_MainMenuButtonEvent -= ResetPlayerHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(1, enemy.transform);
        }
    }

    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth += 3;
            UpdateHealthSlider();
        }
    }

    public  void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage)
        {
            return;
        }

        knockBack.GotKnocked(hitTransform, knockBackThrust);

        StartCoroutine(flash.Flashing());

        canTakeDamage = false;
        currentHealth -= damageAmount;

        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    public void TakeDamage(int damageAmount)
    {
        if (!canTakeDamage)
        {
            return;
        }

        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();

    }

    private void CheckIfPlayerDeath()
    {
        if(currentHealth <= 0 && !IsDead)
        {
            currentHealth = 0;
            IsDead = true;
            OnPlayerDeadEvent?.Invoke();

            if (ActiveWeapon.Instance.transform.childCount > 1)
            {
                Destroy(ActiveWeapon.Instance.transform.GetChild(1).gameObject);
            }

            GetComponent<Animator>().SetTrigger(DEATH_HASH);
        }
    }

    private void ResetPlayerHealth()
    {
        Stamina.Instance.ReplenishStaminaOnDeath();
        currentHealth = maxHealth;
        Invoke("UpdateHealthSlider", 1f);
        IsDead = false;
        Destroy(this.gameObject);

    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
