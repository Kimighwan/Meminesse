using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;
using System.Xml.Serialization;
using UnityEngine.UIElements;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    
    public List<Item>itemDB = new List<Item>();
    [Space(20)]
    public GameObject fieldItemPrefab;

    public Vector3[] dropPos; // 아이템 드랍 위치

    private int dropItemCount = 3; // 추후 값 수정 

    private void Start()
    {
        ItemDrop(); //debug
    }

    public Item GetItemById(int id)
    {
        return itemDB.Find(item => item.itemId == id);
    }

    public void ItemDrop()
    {
        for (int i = 0; i < dropItemCount + 1; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, dropPos[i], Quaternion.identity);
            if (UnityEngine.Random.value < 0.2f)
            {
                go.GetComponent<FieldItems>().SetItem(GetItemById(22)); //마연석
                ItemDataManager.Instance.AddItem(22, 10); // 마연석 10개 추가 - 일단 바로 추가
                Debug.Log("+ 마연석 10개");
            }
            else
            {
                go.GetComponent<FieldItems>().SetItem(GetItemById(21)); //다이아
                ItemDataManager.Instance.AddItem(21, 100); // 다이아 100개 추가 - 일단 바로 추가 
                Debug.Log("+ 다이아 100개");
            }
            Debug.Log("아이템드랍");
        }
    }

}
