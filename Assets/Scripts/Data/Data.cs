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
[System.Serializable]
public class MonsterDropData
{
    public string Name;
    public int DiaProbability;
    public int MaProbability;
    public int DiaCount;
    public int MaCount;
}
[System.Serializable]
public class ShopData
{
    public string ID;
    public int Count;
    public int Price;
}
[System.Serializable]
public class UpgradeData
{
    public string ID;
    public int Count;
}