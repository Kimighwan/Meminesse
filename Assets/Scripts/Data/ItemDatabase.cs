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
        ItemDrop(new Vector3(-2,13,0)); //debug

        ItemDrop(new Vector3(-6,13,0)); //debug
        ItemDrop(new Vector3(-7, 13, 0)); //debug
        ItemDrop(new Vector3(-5, 13, 0)); //debug
        ItemDrop(new Vector3(-4, 13, 0)); //debug
        ItemDrop(new Vector3(-3, 13, 0)); //debug
    }

    public Item GetItemById(int id)
    {
        return itemDB.Find(item => item.itemId == id);
    }

    // 나중에 다른 곳으로 옮기면 좋겠는 메서드
    public void ItemDrop(Vector3 monsterPos)
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 dropPos = monsterPos + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 1f), 0);

            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity); // 
            if (UnityEngine.Random.value < 0.2f)
            {
                go.GetComponent<FieldItems>().SetItem(GetItemById(22)); //마연석
            }
            else
            {
                go.GetComponent<FieldItems>().SetItem(GetItemById(21)); //다이아
            }
        }
    }

}
