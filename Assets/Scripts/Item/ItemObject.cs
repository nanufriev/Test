using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public Item Item { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialize(Item item)
    {
        Item = item;
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.mass = Item.Weight;
        GetComponent<Renderer>().material.color = Item.ItemColor;
    }
}
