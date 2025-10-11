using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemId;
    public string name;
    public string desc;
}
[System.Serializable]
public class InventoryData
{
    public string itemId;
    public ItemData itemData;
    public int count;

    public InventoryData(string itemId, ItemData itemData, int count)
    {
        this.itemId = itemId;
        this.itemData = itemData;
        this.count = count;
    }
}