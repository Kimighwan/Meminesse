using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

// 인벤토리창 아이템 설명칸 관리
public class InventoryItemDescription : SingletonBehaviour<InventoryItemDescription>
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescText;
    [SerializeField] private TextMeshProUGUI noItemNotice;
    [SerializeField] private TextMeshProUGUI upgradeCost;

    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject UseButton;

    private string currentItemId;

    public ItemData GetItemById(string id)
    {
        return DataTableManager.Instance.GetItemData(id);
    }

    public void ShowWeaponDescription()
    {
        int weaponStep = PlayerDataManager.Instance.GetWeaponLevel();
        string itemId = "1" + weaponStep.ToString();
        ItemData item = GetItemById(itemId);

        string costText = null;

        if (item != null)
        {
            if (weaponStep < 5)  // 5 : max weapon level
            {
                int costCount = DataTableManager.Instance.GetUpgradeData(itemId).Count;
                costText = costCount.ToString() + " 필요";
            }

            if (upgradeCost != null && costText != null)
            {
                upgradeCost.text = costText;
            }

            UseButton.SetActive(false);
            upgradeButton.SetActive(weaponStep < 5);

            itemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);

            itemImage.sprite = Resources.Load<Sprite>($"Item/{item.itemId}");
            itemNameText.text = item.name;
            itemDescText.text = item.desc;
        }
        else
        {
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemNameText.text = "/weapon name/";
            itemDescText.text = "/null/";
        }
    }

    public void ShowItemDescription(string itemId)
    {
        currentItemId = itemId;
        //int[] availableToUseIds = { 23, 31, 32, 33 };  
        ItemData item = GetItemById(itemId);

        if (item != null)
        {
            upgradeButton.SetActive(false);
            UseButton.SetActive(true);
            itemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>($"Item/{item.itemId}");
            itemNameText.text = item.name;
            itemDescText.text = item.desc;
        }
        else
        {
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemNameText.text = "/item name/";
            itemDescText.text = "/null/";
        }
    }

    public void HideItemDescription(string itemId)
    {
        currentItemId = null;
        itemImage.gameObject.SetActive(false);
        itemNameText.gameObject.SetActive(false);
        itemDescText.gameObject.SetActive(false);
        upgradeButton.SetActive(false);
        UseButton.SetActive(false);
    }

    // 이 스크립트에 이 함수를 넣는게 맞나...? 검토 요망  
    // 아이템 id에 따른 아이템 효과 발동
    public void ItemUseButton()
    {
        string id = currentItemId;
        InventoryDataManager.Instance.ItemCountReduce(id.ToString(), 1);

        //PrintAllItems();

        // 물약 먹기 - data없이 하드코딩함
        switch (id)
        {
            // 좋은 물약
            case "31":
                Inventory.Instance.inventoryHpBar.Heal(10); break;   
            // 참 좋은 물약
            case "32":
                Inventory.Instance.inventoryHpBar.Heal(40); break;
            // 엄청 좋은 물약
            case "33":
                Inventory.Instance.inventoryHpBar.FullHeal(); break;
            /*
            //최대 체력 증가시키는 아이템
            case "??":
                Inventory.Instance.inventoryHpBar.IncreaseMaxHp(); break;
            */
        }
        if (InventoryDataManager.Instance.GetItemCountById(id.ToString()) == 0)
            HideItemDescription(id);
        InventorySlots.Instance.UpdateInventory();
    }

    //디버그용
    public void PrintAllItems()
    {
        if (InventoryDataManager.Instance.GetItemDataList() == null || InventoryDataManager.Instance.GetItemDataList().Count == 0)
        {
            Debug.Log("인벤토리가 비어있습니다.");
            return;
        }

        Debug.Log("==== 현재 인벤토리 목록 ====");
        foreach (var item in InventoryDataManager.Instance.GetItemDataList())
        {
            Debug.Log($"아이템 ID: {item.itemId}, 개수: {item.count}");
        }
    }

}
