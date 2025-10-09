using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ItemData
{
    public int itemId;
    public int count;


    public ItemData(int itemId, int count)
    {
        this.itemId = itemId;
        this.count = count;
    }
}

[Serializable]
public class WrapperItemDataList
{
    public List<ItemData> itemDataList;
}

public class ItemDataManager : SingletonBehaviour<ItemDataManager>
{
    private List<ItemData> itemDataList = new List<ItemData>();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "itemData.json");
    //private string PATH = Path.Combine(Application.persistentDataPath, "itemData.json");

    protected override void Init()
    {
        base.Init();
        Load();
    }
    public List<ItemData> GetItemDataList()
    {
        return itemDataList;
    }
    public int GetItemCountById(int id)
    {
        return itemDataList.Find(item => item.itemId == id)?.count ?? 0;
    }
    public List<ItemData> SortItem()
    {
        itemDataList.Sort((x, y) => x.itemId.CompareTo(y.itemId));
        return itemDataList;
    }
    public bool ExistItem(int id)
    {
        foreach(var item in itemDataList)
        {
            if(item.itemId == id)
                return true;
        }
        Debug.Log("아이템이 존재하지 않음");
        return false;
    }
    public int ItemCounting(int id)
    {
        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
                return item.count;
        }
        return 0;
    }

    public bool ItemCountReduce(int id, int count)
    {
        if (!ExistItem(id)) return false;

        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
            {
                item.count -= count;
                if(item.count <= 0)
                {
                    itemDataList.Remove(item);
                    return true;
                }
            }
        }

        return false;
    }
    public bool AddItem(int id, int count)
    {
        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
            {
                item.count += count;  
                return true;
            }
        }

        itemDataList.Add(new ItemData(id, count));
        return true;
    }

    #region Save-Load

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
            Save();
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
