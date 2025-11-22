using System;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;
    [SerializeField] private int _startingCoins = 0;
    private const int _coinsToAdd = 10;

    public static CurrencyManager Instance;

    public int Coins { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Coins = _startingCoins;

        UpdateText();
    }

    public void AddCoins(int amount = _coinsToAdd)
    {
        Coins += amount;
        UpdateText();
    }

    private void UpdateText()
    {
        _currencyText.text = Coins.ToString();
    }

    internal bool TrySpendCurrency(int price)
    {
        if (Coins >= price)
        {
            Coins -= price;
            return true;
        }
        return false;
    }

}
