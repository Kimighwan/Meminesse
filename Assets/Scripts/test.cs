using UnityEngine;

public class test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            //PlayerDataManager.Instance.SetDefaultData();
            TestAddMoney();
            TestAddBluePotion();
            TestAddYellowPotion();
            TestAddRedPotion();
            TestAddInventoryItems();
        }
    }

    // 돈(빨간색마연석) 1000 추가
    public void TestAddMoney()
    {
        var itemData = DataTableManager.Instance.GetItemData("22");

        InventoryDataManager.Instance.AddItem(itemData, 1000);
        Inventory.Instance.RefreshInventory();
    }

    // 파란물약 추가
    public void TestAddBluePotion()
    {
        var itemData = DataTableManager.Instance.GetItemData("31");
        InventoryDataManager.Instance.AddItem(itemData, 1);
        Inventory.Instance.RefreshInventory();
    }

    // 노란물약 추가
    public void TestAddYellowPotion()
    {
        var itemData = DataTableManager.Instance.GetItemData("32");
        InventoryDataManager.Instance.AddItem(itemData, 1);
        Inventory.Instance.RefreshInventory();
    }

    // 빨간물약 추가
    public void TestAddRedPotion()
    {
        var itemData = DataTableManager.Instance.GetItemData("33");
        InventoryDataManager.Instance.AddItem(itemData, 1);
        Inventory.Instance.RefreshInventory();
    }

    // 인벤토리 아이템 몇개 넣어주기
    public void TestAddInventoryItems()
    {
        var itemData1 = DataTableManager.Instance.GetItemData("21");
        InventoryDataManager.Instance.AddItem(itemData1, 100);
        var itemData2 = DataTableManager.Instance.GetItemData("31");
        InventoryDataManager.Instance.AddItem(itemData2, 10);
        var itemData3 = DataTableManager.Instance.GetItemData("32");
        InventoryDataManager.Instance.AddItem(itemData3, 5);
        var itemData4 = DataTableManager.Instance.GetItemData("33");
        InventoryDataManager.Instance.AddItem(itemData4, 2);
        var itemData5 = DataTableManager.Instance.GetItemData("53");
        InventoryDataManager.Instance.AddItem(itemData5, 1);
        var itemData6 = DataTableManager.Instance.GetItemData("31");
        InventoryDataManager.Instance.AddItem(itemData6, 3);
        var itemData7 = DataTableManager.Instance.GetItemData("43");
        InventoryDataManager.Instance.AddItem(itemData7, 4);
        Inventory.Instance.RefreshInventory();
    }
}
