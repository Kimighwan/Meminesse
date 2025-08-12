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
    public List<ItemData> itemDataList = new List<ItemData>();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH;

    protected override void Init()
    {
        base.Init();

        // 경로는 임시로 dataPath로 지정 - 테스터 용이, 추후 아래 주석으로 적용하기
        PATH = Path.Combine(Application.dataPath, "itemData.json");
        //PATH = Path.Combine(Application.persistentDataPath, "itemData.json");
        Load();
        Save();
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
        if (ExistItem(id)) return false;

        itemDataList.Add(new ItemData(id, count));
        return true;
    }
    public bool ItemCountIncrease(int id, int count)
    {
        if (!ExistItem(id))
        {
            itemDataList.Add(new ItemData(id, count));
            return false;
        }

        foreach (var item in itemDataList)
        {
            if (item.itemId == id)
            {
                item.count += count;
                return true;
            }
        }

        return false;
    }

    #region Save-Load

    public void Save()
    {
        WrapperItemDataList itemDataListWrapper = new WrapperItemDataList();
        itemDataListWrapper.itemDataList = itemDataList;
        string jsonItemDataListWrapper = JsonUtility.ToJson(itemDataListWrapper);
        File.WriteAllText(PATH, jsonItemDataListWrapper);
        //File.WriteAllText(PATH, Encrypt(jsonItemDataListWrapper, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            Save();
        }
        else // Load
        {
            Debug.Log("파일 존재해서 로드");
            string jsonItemDataListWrapper = File.ReadAllText(PATH);
            WrapperItemDataList itemDataListWrapper = JsonUtility.FromJson<WrapperItemDataList>(jsonItemDataListWrapper);
            //WrapperItemDataList itemDataListWrapper = JsonUtility.FromJson<WrapperItemDataList>(Decrypt(jsonItemDataListWrapper, KEY));
            itemDataList = itemDataListWrapper.itemDataList;
            Save();
        }
    }
    #endregion
    #region Security
    private static string Encrypt(string plainText, string key)
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

    private static string Decrypt(string cipherText, string key)
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

    private static byte[] AdjustKeyLength(byte[] keyBytes)
    {
        byte[] adjustedKey = new byte[32];
        Array.Copy(keyBytes, adjustedKey, Math.Min(keyBytes.Length, 32));
        return adjustedKey;
    }
    #endregion
}
