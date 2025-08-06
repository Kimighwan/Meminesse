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
    public SaveFileData saveFileData;

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH;

    protected override void Init()
    {
        base.Init();

        // 경로는 임시로 dataPath로 지정 - 테스터 용이, 추후 아래 주석으로 적용하기
        PATH = Path.Combine(Application.dataPath, "saveFileData.json");
        //PATH = Path.Combine(Application.persistentDataPath, "saveFileData.json");
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
        saveFileData.mapId.Add(addId);
        Save();
    }

    #endregion
    #region Save-Load

    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveFileData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            Debug.Log("파일 없어서 새로 저장");
            saveFileData = new SaveFileData();
            Save();
        }
        else // Load
        {
            Debug.Log("파일 존재해서 로드");
            string loadJson = File.ReadAllText(PATH);
            saveFileData = JsonUtility.FromJson<SaveFileData>(Decrypt(loadJson, KEY));
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
