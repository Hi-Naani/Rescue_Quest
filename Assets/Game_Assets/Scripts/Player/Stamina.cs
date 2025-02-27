using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina> 
{

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;

    public int CurrentStamina { get; private set; }

    const string STAMINA_CONTAINER_TEXT = "/UICanvas/GamePlayUI/Stamina Contanier";

    protected override void Awake()
    {
        base.Awake();

        maxStamina = startingStamina;
        CurrentStamina = startingStamina;
    }

    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImage();
        StopAllCoroutines();
        StartCoroutine(RefreshStaminaRoutine());
    }

    public void RefreshStamina()
    {
        if(CurrentStamina < maxStamina && !PlayerHealth.Instance.IsDead)
        {
            CurrentStamina++;
        }

        UpdateStaminaImage();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamina();
        }
    }

    public void ReplenishStaminaOnDeath()
    {
        CurrentStamina = startingStamina;
        //UpdateStaminaImage();
        Invoke("UpdateStaminaImage", 1f);
    }

    private void UpdateStaminaImage()
    {
        for(int i = 0; i < maxStamina; i++)
        {
            Transform child = staminaContainer.GetChild(i);
            Image image = child?.GetComponent<Image>();

            if(i <= CurrentStamina - 1)
            {
                image.sprite = fullStaminaImage;
            }
            else
            {
                image.sprite = emptyStaminaImage;
            }
        }
    }


}
