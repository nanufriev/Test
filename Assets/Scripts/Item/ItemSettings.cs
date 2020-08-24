using UnityEngine;

[CreateAssetMenu(fileName = "ItemSettings", menuName = "TestGame/Settings/ItemSettings")]
public class ItemSettings : ScriptableObject
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private string _name;
    [SerializeField]
    private float _weight;
    [SerializeField]
    private Sprite _icon;

    public int ItemID => _id;
    public string Name => _name;
    public float Weight => _weight;
    public Sprite Icon => _icon;
}
