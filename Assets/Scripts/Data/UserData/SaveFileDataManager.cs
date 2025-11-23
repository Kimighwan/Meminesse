using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class SaveFileData
{
    public int fileIndex;       // 몇 번째 세이브 파일
    public List<int> mapId;     // 무슨 맵을 방문했는지 확인을 위한 배열
    public int lastMapId;       // 마지막에 있던 맵 ID 저장
    public SaveFileData()
    {
        fileIndex = 0;
        mapId = new List<int>();
        lastMapId = 0;
    }
    public SaveFileData(SaveFileData data)
    {
        fileIndex = data.fileIndex;
        mapId = data.mapId.ToList();
        lastMapId = data.lastMapId;
    }
}

public class SaveFileDataManager : SingletonBehaviour<SaveFileDataManager>
{
    private SaveFileData saveFileData = new();

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "saveFileData.json");
    //PATH = Path.Combine(Application.persistentDataPath, "saveFileData.json");

    protected override void Init()
    {
        base.Init();

        Load();
    }

    #region Get

    public int GetLastMapId()
    {
        return saveFileData.lastMapId;
    }
    public int GetFileIndex()
    {
        return saveFileData.fileIndex;
    }
    public int[] GetVisitedMapId()
    {
        return saveFileData.mapId.ToArray();
    }

    #endregion
    #region Set

    public void SetLastMapId(int id)
    {
        saveFileData.lastMapId = id;
        Save();
    }
    public void AddVisitedMapId(int addId)
    {
        if (saveFileData.mapId.Contains(addId)) return;

        saveFileData.mapId.Add(addId);
        Save();
    }
    #endregion

    #region Save-Load

    public void SetDefaultData()
    {
        saveFileData = new SaveFileData();
        saveFileData.mapId.Add(1);
        Save();
    }

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveFileData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            SetDefaultData(); 
        }
        else // Load
        {
            string loadJson = File.ReadAllText(PATH);
            saveFileData = JsonUtility.FromJson<SaveFileData>(Decrypt(loadJson, KEY));
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
