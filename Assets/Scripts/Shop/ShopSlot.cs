using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _buyButton;

    private ShopItem _item;

    public void Setup(ShopItem item, Action<ShopItem, ShopSlot> onBuy)
    {
        _item = item;

        _descriptionText.text = item.Description;   
        _priceText.text = item.Price.ToString();
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.interactable = true;
        _buyButton.onClick.AddListener(() => onBuy?.Invoke(item, this));
        _buyButton.onClick.AddListener(OnBuy);
    }

    private void OnBuy()
    {
        _buyButton.interactable = false;
    }
}