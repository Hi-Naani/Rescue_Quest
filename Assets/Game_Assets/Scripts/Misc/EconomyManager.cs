using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;
    public int CurrentGold { get { return currentGold; } private set { } }

    const string COIN_AMOUNT_TEXT = "/UICanvas/GamePlayUI/GoldCoin Container/Gold Coin Count Text";

    public void UpdateCurrentGold()
    {
        currentGold += 1;

        if(goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }

}
