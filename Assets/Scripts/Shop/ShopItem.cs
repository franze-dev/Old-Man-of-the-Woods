using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    [SerializeField, TextArea] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private int _level;
    [SerializeField] private ImprovementEffect _effect;

    public int Level => _level;

    public string Description => _description;

    public int Price => _price;

    public ImprovementEffect Effect => _effect;
}