using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using TMPro;

// 인벤토리창 아이템 슬롯창 관리
public class InventoryUI : MonoBehaviour
{

    public Slot[] slots; // 인벤토리 슬롯 배열
    public Transform slotHolder; // 슬롯을 담고 있는 부모 오브젝트

    [SerializeField]
    private TextMeshProUGUI noItemMessage;

    #region Singleton
    public static InventoryUI Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>(); // 자식 오브젝트 가져오기
        UpdateInventory();
    }
    
    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
        }
    }

 

    //슬롯에 아이템 추가하는 함수      <<  플레이어가 아이템 먹을 때 사용
    public void AddItemToInventory(ItemData itemData)
    {
        InventoryDataManager.Instance.AddItem(itemData, 1);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        // itemDataList를 가져와 인벤토리 UI 업데이트
        List<InventoryData> inventoryItemDataList = InventoryDataManager.Instance.GetItemDataList();
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventoryItemDataList.Count)
            {
                InventoryData inventoryItemData = inventoryItemDataList[i];
                if (inventoryItemData.itemId == "21" || inventoryItemData.itemId == "22") // 돈 아이템은 인벤토리에 표시x
                {
                    slots[i].ClearSlot();
                    slots[i].gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    ItemData itemData = inventoryItemData.itemData;
                    slots[i].gameObject.SetActive(true);
                    slots[i].SetSlot(itemData, inventoryItemData.count);
                }
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].gameObject.SetActive(false);
            }
        }

        // 인벤토리가 비어있으면
        if (inventoryItemDataList.Count == 0)
        {
            noItemMessage.gameObject.SetActive(true);
        }
        else
        {
            noItemMessage.gameObject.SetActive(false);
        }


    }

    // 디버그용 - 랜덤 아이템 추가(무기, 돈 제외) ------------ 나중에 삭제함
    public void DebugAddRandomItem()
    {
        int[] possibleItems = { 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 51, 52, 53 };

        int randomIndex = Random.Range(0, possibleItems.Length);
        int randomId = possibleItems[randomIndex];

        var itemData = DataTableManager.Instance.GetItemData(randomId.ToString());

        InventoryDataManager.Instance.AddItem(itemData, 1);
        UpdateInventory();
    }


}
