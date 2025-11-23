using System.Collections.Generic;
using System;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    protected override void Init()
    {
        base.Init();

        LoadItemDataTable();
        LoadMonsterDropDataTable();
        LoadShopDataTable();
        LoadUpgradeDataTable();
    }

    #region Item Data

    private const string ITEM_DATA_TABLE = "ItemDataTable";
    private List<ItemData> ItemDataTable = new List<ItemData>();

    private void LoadItemDataTable()
    {
        var persedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        foreach (var data in persedDataTable)
        {
            var itemData = new ItemData
            {
                itemId = data["id"].ToString(),
                name = data["name"].ToString(),
                desc = data["desc"].ToString(),
            };
            ItemDataTable.Add(itemData);
        }
    }

    public ItemData GetItemData(string id)
    {
        return ItemDataTable.Where(item => item.itemId == id).FirstOrDefault();
    }

    #endregion

    #region Monster Drop Item

    private const string MD_DATA_TABLE = "MonsterDropDataTable";
    private List<MonsterDropData> MonsterDropDataTable = new List<MonsterDropData>();

    private void LoadMonsterDropDataTable()
    {
        var persedDataTable = CSVReader.Read($"{DATA_PATH}/{MD_DATA_TABLE}");

        foreach (var data in persedDataTable)
        {
            var monsterDropData = new MonsterDropData
            {
                Name = data["Name"].ToString(),
                DiaProbability = Convert.ToInt32(data["DiaProbability"]),
                MaProbability = Convert.ToInt32(data["MaProbability"]),
                DiaCount = Convert.ToInt32(data["DiaCount"]),
                MaCount = Convert.ToInt32(data["MaCount"]),
            };
            MonsterDropDataTable.Add(monsterDropData);
        }
    }

    public MonsterDropData GetMonsterDropData(string monsterName)
    {
        return MonsterDropDataTable.Where(item => item.Name == monsterName).FirstOrDefault();
    }

    #endregion

    #region Shop Data

    private const string SHOP_DATA_TABLE = "ShopDataTable";
    private List<ShopData> ShopDataTable = new List<ShopData>();

    private void LoadShopDataTable()
    {
        var persedDataTable = CSVReader.Read($"{DATA_PATH}/{SHOP_DATA_TABLE}");

        foreach (var data in persedDataTable)
        {
            var shopData = new ShopData
            {
                ID = data["ID"].ToString(),
                Count = Convert.ToInt32(data["Count"]),
                Price = Convert.ToInt32(data["Price"]),
            };
            ShopDataTable.Add(shopData);
        }
    }

    public ShopData GetShopData(string itemID)
    {
        return ShopDataTable.Where(item => item.ID == itemID).FirstOrDefault();
    }

    #endregion

    #region Upgrade

    private const string UPGRADE_DATA_TABLE = "UpgradeDataTable";
    private List<UpgradeData> UpgradeDataTable = new List<UpgradeData>();

    private void LoadUpgradeDataTable()
    {
        var persedDataTable = CSVReader.Read($"{DATA_PATH}/{UPGRADE_DATA_TABLE}");

        foreach (var data in persedDataTable)
        {
            var upgradeData = new UpgradeData
            {
                ID = data["ID"].ToString(),
                Count = Convert.ToInt32(data["Count"]),
            };
            UpgradeDataTable.Add(upgradeData);
        }
    }

    public UpgradeData GetUpgradeData(string itemID)
    {
        return UpgradeDataTable.Where(item => item.ID == itemID).FirstOrDefault();
    }

    #endregion
}
