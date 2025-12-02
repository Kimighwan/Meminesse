using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

[Serializable]
public class WrapperItemDataList
{
    public List<InventoryData> itemDataList;
}

public class InventoryDataManager : SingletonBehaviour<InventoryDataManager>
{
    private List<InventoryData> itemDataList = new List<InventoryData>();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "Data/inventoryData.json");
    //private string PATH = Path.Combine(Application.persistentDataPath, "inventoryData.json");

    protected override void Init()
    {
        base.Init();
        Load();
    }
    public List<InventoryData> GetItemDataList()
    {
        return itemDataList;
    }


    public int GetItemCountById(string id)
    {
        return itemDataList.Find(item => item.itemId == id)?.count ?? 0;
    }

    public List<InventoryData> SortItem()
    {
        itemDataList.Sort((x, y) => x.itemId.CompareTo(y.itemId));
        return itemDataList;
    }
    public bool ExistItem(string id)
    {
        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
                return true;
        }
        return false;
    }
    public int ItemCounting(string id)
    {
        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
                return item.count;
        }
        return 0;
    }

    public bool ItemCountReduce(string id, int count)
    {
        if (!ExistItem(id)) return false;

        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
            {
                item.count -= count;
                if (item.count <= 0)
                {
                    itemDataList.Remove(item);
                    return true;
                }
            }
        }

        return false;
    }
    public bool AddItem(ItemData itemData, int count)
    {
        foreach (var item in itemDataList)
        {
            if (item.itemId == itemData.itemId)
            {
                item.count += count;
                return true;
            }
        }

        itemDataList.Add(new InventoryData(itemData.itemId, itemData, count));
        return true;
    }
    #region Save-Load

    public void SetDefaultData()
    {
        itemDataList = new List<InventoryData>();
        Save();
    }

    public void Save()
    {
        WrapperItemDataList itemDataListWrapper = new WrapperItemDataList();
        itemDataListWrapper.itemDataList = itemDataList;
        string jsonItemDataListWrapper = JsonUtility.ToJson(itemDataListWrapper);
        File.WriteAllText(PATH, Encrypt(jsonItemDataListWrapper, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            SetDefaultData();
        }
        else // Load
        {
            string jsonItemDataListWrapper = File.ReadAllText(PATH);
            WrapperItemDataList itemDataListWrapper = JsonUtility.FromJson<WrapperItemDataList>(Decrypt(jsonItemDataListWrapper, KEY));
            itemDataList = itemDataListWrapper.itemDataList;
        }
    }
    #endregion
    #region Security
    protected string Encrypt(string plainText, string key)
    {
        byte[] keyBytes = AdjustKeyLength(Encoding.UTF8.GetBytes(key));

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            byte[] iv = aes.IV;
            using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(iv, 0, iv.Length);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    protected string Decrypt(string cipherText, string key)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        byte[] keyBytes = AdjustKeyLength(Encoding.UTF8.GetBytes(key));
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
            using (var memoryStream = new MemoryStream(cipher))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    protected byte[] AdjustKeyLength(byte[] keyBytes)
    {
        byte[] adjustedKey = new byte[32];
        Array.Copy(keyBytes, adjustedKey, Math.Min(keyBytes.Length, 32));
        return adjustedKey;
    }
    #endregion
}
