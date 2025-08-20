using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    private Item itemInfo;
    private int itemCount;

    public void SetSlot(Item newItem, int count)
    {
        itemInfo = newItem;
        itemCount = count;

        if (itemInfo != null)
        {
            icon.sprite = itemInfo.itemImage;
            icon.enabled = true;
            countText.text = count > 1 ? count.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        itemInfo = null;
        itemCount = 0;
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }

    public void OnClick()
    {
        if (itemInfo == null)
        {
            Debug.LogWarning("슬롯에 아이템 정보가 없음!");
            return;
        }
        InventoryItemDescription.Instance.ShowItemDescription(itemInfo.itemId);
    }
}
