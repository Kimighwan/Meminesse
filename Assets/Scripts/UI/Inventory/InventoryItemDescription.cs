using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

// 인벤토리창 아이템 설명칸 관리
public class InventoryItemDescription : MonoBehaviour
{
    
    #region Singleton
    public static InventoryItemDescription Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
   

    public ItemDatabase itemDatabase;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemDescText;

    [SerializeField]
    private GameObject upgradeButton;
    [SerializeField]
    private GameObject UseButton;

    private int currentItemId = -1;

    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItemById(int id)
    {
        return itemDatabase.itemDB.Find(item => item.itemId == id);
    }

    public void ShowWeaponDescription()
    {
        int weaponStep = PlayerDataManager.Instance.GetWeaponStep();
        Item item = null;

        switch(weaponStep)
        {
            case 1:
                item = GetItemById(11);
                break;
            case 2:
                item = GetItemById(12);
                break;
            case 3:
                item = GetItemById(13);
                break;
            case 4:
                item = GetItemById(14);
                break;
            case 5:
                item = GetItemById(15);
                break;  
        }
        
        if (item != null)
        {
            UseButton.SetActive(false);
            upgradeButton.SetActive(true);
            itemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemImage.sprite = item.itemImage; // 아이템 이미지 설정
            itemNameText.text = item.itemName;
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

    public void ShowItemDescription(int itemId)
    {
        currentItemId = itemId;
        //int[] availableToUseIds = { 23, 31, 32, 33 };  
        Item item = GetItemById(itemId);

        if (item != null)
        {
            upgradeButton.SetActive(false);
            UseButton.SetActive(true);
            itemImage.gameObject.SetActive(true);
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemImage.sprite = item.itemImage;
            itemNameText.text = item.itemName;
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

    public void HideItemDescription(int itemId)
    {
        currentItemId = -1;
        itemImage.gameObject.SetActive(false);
        itemNameText.gameObject.SetActive(false);
        itemDescText.gameObject.SetActive(false);
        upgradeButton.SetActive(false);
        UseButton.SetActive(false);
    }

    // 아이템 id에 따른 아이템 효과 발동
    public void ItemUseButton()
    {
        int id = currentItemId;
        ItemDataManager.Instance.ItemCountReduce(id, ItemDatabase.instance.GetItemById(id).count);

        switch(id)
        {
            // 좋은 물약
            case 31:
                HpUIManager.Instance.Heal(10); break;   
            // 참 좋은 물약
            case 32:
                HpUIManager.Instance.Heal(40); break;
            // 엄청 좋은 물약
            case 33:
                HpUIManager.Instance.FullHeal(); break;
        }
    }
}
