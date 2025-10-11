using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    protected override void Init()
    {
        base.Init();

        LoadItemDataTable();
    }

    #region Item_Data

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

    #region Shop

    #endregion
}
