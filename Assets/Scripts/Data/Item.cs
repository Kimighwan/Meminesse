using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Item
{
    public string itemName;   // 아이템 이름
    public Sprite itemImage;  // 아이템 이미지 (UI 표시용)
    public int itemId;        // 아이템 ID
    public int count;     // 아이템 개수
    public string desc; // 아이템 설명

    public bool Use()
    {
        return false;
    }

}
