using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveFileData
{
    public int fileIndex;       // 몇 번째 세이브 파일
    public List<int> mapId;     // 무슨 맵을 갔는지 확인을 위한 맵 ID들 저장
    public int lastMapId;       // 마지막에 있던 맵 ID 저장
    public SaveFileData()
    {
        fileIndex = 0;
        mapId = new List<int>();
        lastMapId = 0;
    }
}

public class SaveFileDataManager : SingletonBehaviour<SaveFileDataManager>
{
    public SaveFileData saveFileData = new SaveFileData();

    public void Awake()
    {
        saveFileData.fileIndex = 20040604;
        saveFileData.mapId.Add(1);
        saveFileData.mapId.Add(2);
        saveFileData.mapId.Add(3);
        saveFileData.mapId.Add(4);
        saveFileData.mapId.Add(5);
        saveFileData.lastMapId = 20040604;

        string jsonData = JsonUtility.ToJson(saveFileData);
        File.WriteAllText(Path.Combine(Application.dataPath, "saveFileData.json"), jsonData);
        Debug.Log(jsonData);
    }
}
