using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public Slot[] slots; // 인벤토리 슬롯 배열
    public Transform slotHolder; // 슬롯을 담고 있는 부모 오브젝트

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        inven = Inventory.instance; // 인벤토리 인스턴스 가져오기
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange; // 슬롯 개수가 바뀌면 나중에 onSlotCountChange(val)가 실행될 때 SlotChange(val)가 자동으로 불림
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true; // 슬롯 활성화
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false; // 슬롯 비활성화
            }
        }
    }

    //슬롯에 아이템 추가하는 함수
    public void AddSlot(int id, int count)
    {
        inven.SlotCnt++; // 슬롯 개수 증가
        ItemDataManager.Instance.AddItem(id, count);
    }

}
