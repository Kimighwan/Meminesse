using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

// 인벤토리창 아이템 설명칸 관리
public class InventoryItemDescription : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private TextMeshProUGUI itemNameText;
    [SerializeField]
    private TextMeshProUGUI itemDescText;

    void Start()
    {
       
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public Item GetItemById(int id)
    {
        // itemDatabase(전체 아이템 목록)에서 찾지 않고
        // itemDataList(사용자가 갖고있는 아이템 목록)에서 찾을 것임
        return itemDatabase.itemDB.Find(item => item.itemId == id);
    }

    public void ShowWeaponDescription(int itemId)
    {
        // 이 함수는 인수를 전달받지 않을건데 임시로 넣어둠.
        // 현재 사용자가 갖고있는 무기(itemId 11~15) 중 가장 높은 단계의 무기를 띄울 것임
        Item item = GetItemById(itemId);
        if (item != null)
        {
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
            itemDescText.text = "null null null";
        }
    }

    public void ShowItemDescription(int itemId)
    {
        Item item = GetItemById(itemId);
        if (item != null)
        {
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemNameText.text = item.itemName;
            itemDescText.text = item.desc;
        }
        else
        {
            itemNameText.gameObject.SetActive(true);
            itemDescText.gameObject.SetActive(true);
            itemNameText.text = "/item name/";
            itemDescText.text = "null null null";
        }
    }
}
