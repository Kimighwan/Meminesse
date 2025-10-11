using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    private ItemData itemData;
    private int itemCount;

    public void SetSlot(ItemData newItem, int count)
    {
        itemData = newItem;
        itemCount = count;

        if (itemData != null)
        {
            icon.sprite = Resources.Load<Sprite>($"Item/{itemData.itemId}");
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
        itemData = null;
        itemCount = 0;
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }

    public void OnClick()
    {
        if (itemData == null)
        {
            Debug.LogWarning("슬롯에 아이템 정보가 없음!");
            return;
        }
        InventoryItemDescription.Instance.ShowItemDescription(itemData.itemId);
    }
}
