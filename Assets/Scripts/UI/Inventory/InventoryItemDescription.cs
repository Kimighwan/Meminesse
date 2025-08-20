using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

// 인벤토리창 아이템 설명칸 관리
public class InventoryItemDescription : MonoBehaviour
{
    
    #region Singleton
    public static InventoryItemDescription instance;
    private void Awake()
    {
        instance = this;
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
}
