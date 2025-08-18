using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

// 인벤토리창 아이템 슬롯창 관리
public class InventoryUI : MonoBehaviour
{

    public Slot[] slots; // 인벤토리 슬롯 배열
    public Transform slotHolder; // 슬롯을 담고 있는 부모 오브젝트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        slots = slotHolder.GetComponentsInChildren<Slot>(); // 자식 오브젝트 가져오기
        Inventory.instance.onSlotCountChange += SlotChange; // 슬롯 개수가 바뀌면 나중에 onSlotCountChange(val)가 실행될 때 SlotChange(val)가 자동으로 불림
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.SlotCnt) //SlotCnt수만큼만 slot ui 활성화
            {
                slots[i].gameObject.SetActive(true);  // 슬롯 오브젝트 보이기
                //Debug.Log("ui 활성화"); // 확인용
            }
            else
            {
                slots[i].gameObject.SetActive(false); // 슬롯 오브젝트 숨기기
            }
        }
    }

    //슬롯에 아이템 추가하는 함수
    public void AddSlot(int id)
    {
        Inventory.instance.SlotCnt++; // 슬롯 개수 증가
        ItemDataManager.Instance.AddItem(id, 1);
        Debug.Log($"슬롯 수 : {Inventory.instance.SlotCnt}");
    }

}
