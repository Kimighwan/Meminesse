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

    private int dropItemCount = 3; // 드랍아이템수 -> 추후 값 수정 


    private void Start()
    {
        ItemDrop(new Vector3(-2,12,0)); //debug

        ItemDrop(new Vector3(0,12,0)); //debug
    }

    public Item GetItemById(int id)
    {
        return itemDB.Find(item => item.itemId == id);
    }

    public void ItemDrop(Vector3 monsterPos)
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 dropPos = monsterPos + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0);

            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);
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
