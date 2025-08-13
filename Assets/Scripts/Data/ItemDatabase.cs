using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<Item>itemDB = new List<Item>();
}
