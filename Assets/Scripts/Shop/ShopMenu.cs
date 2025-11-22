using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private List<ShopItem> _allItems = new();
    [SerializeField] private List<ShopSlot> _slots = new();
    [SerializeField] private GameObject _slotParent;
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    [SerializeField] private Color _noGoldColor = Color.red;
    [SerializeField] private float _noGoldDuration = 1.5f;
    private Player _player;
    private Color _originalGoldColor;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        ServiceProvider.SetService(this);
    }

    private void Start()
    {
        _originalGoldColor = _playerGoldText.color;
        ServiceProvider.TryGetService(out _player);
        RefreshGold();
    }

    private void RefreshGold()
    {
        if (_playerGoldText && CurrencyManager.Instance)
            _playerGoldText.text = CurrencyManager.Instance.Coins.ToString();
    }

    public void OpenShop(int currentLevel)
    {
        RefreshGold();

        var items = _allItems.Where(i => i.Level == currentLevel).ToList(); 

        for (int i = 0; i < items.Count && i < _slots.Count; i++)
        {
            var item = items[i];
            var slot = _slots[i];
            slot.Setup(item, OnBuy);
        }
    }

    private void OnBuy(ShopItem item, ShopSlot slot)
    {
        if (CurrencyManager.Instance.TrySpendCurrency(item.Price))
        {
            if (_player == null)
                ServiceProvider.TryGetService(out _player);
            item.Effect.Apply(_player);
            RefreshGold();
        }
        else
        {
            if (_flashCoroutine!= null)
                StopCoroutine(_flashCoroutine);

            _flashCoroutine = StartCoroutine(InsufficientGoldCoroutine());
        }
    }

    private IEnumerator InsufficientGoldCoroutine()
    {
        _playerGoldText.color = _noGoldColor;

        yield return new WaitForSeconds(0.5f);

        _playerGoldText.color = _originalGoldColor;

        yield return new WaitForSeconds(0.3f);

        _playerGoldText.color = _noGoldColor;

        yield return new WaitForSeconds(0.5f);

        _playerGoldText.color = _originalGoldColor;
        _flashCoroutine = null;
    }
}
