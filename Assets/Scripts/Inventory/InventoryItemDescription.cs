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

    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemData GetItemById(string id)
    {
        return DataTableManager.Instance.GetItemData(id);
    }

    public void ShowWeaponDescription()
    {
        int weaponStep = PlayerDataManager.Instance.GetWeaponLevel();
        ItemData item = null;

        switch(weaponStep)
        {
            // 낡은 검
            case 1:
                item = GetItemById("11");
                upgradeCost.text = "200 필요";
                break;
            // 평범한 검
            case 2:
                item = GetItemById("12");
                upgradeCost.text = "500 필요";
                break;
            // 단련된 검
            case 3:
                item = GetItemById("13");
                upgradeCost.text = "1000 필요";
                break;
            // 불멸의 검
            case 4:
                item = GetItemById("14");
                upgradeCost.text = "5000 필요";
                break;
            // 만계황혼의 심연룡섬검
            case 5:
                item = GetItemById("15");
                break;  
        }
        
        if (item != null)
        {
            UseButton.SetActive(false);
            if(item.itemId != "15") 
                upgradeButton.SetActive(true); //최고 레벨이라서 업그레이드 불가
            else
                upgradeButton.SetActive(false);

            itemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemImage.sprite = Resources.Load<Sprite>($"Item/{item.itemId}"); // 아이템 이미지 설정
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
       
        InventoryUI.Instance.UpdateInventory();
        switch (id)
        {
            // 좋은 물약
            case "31":
                HpUIManager.Instance.Heal(10); break;   
            // 참 좋은 물약
            case "32":
                HpUIManager.Instance.Heal(40); break;
            // 엄청 좋은 물약
            case "33":
                HpUIManager.Instance.FullHeal(); break;
        }
        if (InventoryDataManager.Instance.GetItemCountById(id.ToString()) == 0)
            HideItemDescription(id);

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
