using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class FieldItems : MonoBehaviour
{
    public Item item;

    public SpriteRenderer spriteRenderer;

    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemId = _item.itemId;
        item.itemImage = _item.itemImage;
        item.desc = _item.desc;
        item.itemImage = _item.itemImage;

        if (spriteRenderer != null && item.itemImage != null)
        {
            spriteRenderer.sprite = item.itemImage;
        }
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
