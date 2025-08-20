using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Slax.InventorySystem.Runtime.Core;
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

    }

 

    //슬롯에 아이템 추가하는 함수      <<  플레이어가 아이템 먹을 때 사용
    public void AddItemToInventory(int id)
    {
        ItemDataManager.Instance.AddItem(id, 1);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        // itemDataList를 가져와 인벤토리 UI 업데이트
        List<ItemData> itemDataList = ItemDataManager.Instance.GetItemDataList();
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < itemDataList.Count)
            {
                ItemData data = itemDataList[i];
                Item itemInfo = Inventory.Instance.itemDatabase.GetItemById(data.itemId);
                slots[i].gameObject.SetActive(true);
                slots[i].SetSlot(itemInfo, data.count);
            }
            else
            {
                slots[i].ClearSlot();
                slots[i].gameObject.SetActive(false);
            }
        }

        // 인벤토리가 비어있으면
        if (itemDataList.Count == 0)
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

        ItemDataManager.Instance.AddItem(randomId, 1);
        UpdateInventory();
    }


}
