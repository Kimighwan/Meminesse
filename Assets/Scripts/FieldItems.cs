using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class FieldItems : MonoBehaviour
{
    public Item item;

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemId = _item.itemId;
        item.itemImage = _item.itemImage;
        item.desc = _item.desc;
        item.itemImage = _item.itemImage;
    }

    public Item GetItem()
    {
        return item;
    }    

    public void DestroyItem()
    {
        Destroy(gameObject);
    }    
}
