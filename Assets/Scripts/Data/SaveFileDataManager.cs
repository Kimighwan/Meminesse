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

public class SaveFileDataManager : Security, ISaveAndLoad
{
    private SaveFileData saveFileData;

    private const string KEY = "Ikhwan@@ZZang!!";
    private string PATH = Path.Combine(Application.dataPath, "saveFileData.json");
    //PATH = Path.Combine(Application.persistentDataPath, "saveFileData.json");

    public void Init()
    {
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
    public void Save()
    {
        string jsonData = JsonUtility.ToJson(saveFileData);
        File.WriteAllText(PATH, Encrypt(jsonData, KEY));
    }
    public void Load()
    {
        if (!File.Exists(PATH)) // Create
        {
            saveFileData = new SaveFileData();
            saveFileData.mapId.Add(1);
            Save();
        }
        else // Load
        {
            string loadJson = File.ReadAllText(PATH);
            saveFileData = JsonUtility.FromJson<SaveFileData>(Decrypt(loadJson, KEY));
        }
    }
    #endregion
}
