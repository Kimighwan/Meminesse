using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    private List<ISaveAndLoad> datas = new List<ISaveAndLoad>();

    PlayerDataManager _player = new();
    SaveFileDataManager _saveFile = new();
    ItemDataManager _item = new ();
    SettingDataManager _setting = new ();

    public static DataManager Instance { get { return _instance; } }
    public static PlayerDataManager Player { get { return _instance._player; } }
    public static SaveFileDataManager SaveFile { get { return _instance._saveFile; } }
    public static ItemDataManager Item { get { return _instance._item; } }
    public static SettingDataManager Setting { get { return _instance._setting; } }

    public static void Init()
    {
        if(_instance == null)
        {
            _instance.datas.Add(Player);
            _instance.datas.Add(Item);
            _instance.datas.Add(Setting);
            _instance.datas.Add(SaveFile);

            GameObject go = GameObject.Find("DataManager");
            if (go == null)
                go = new GameObject("DataManager");

            _instance = go.AddComponent<DataManager>();
            DontDestroyOnLoad(go);
        }
    }

    public void AllSave()
    {
        for(int i = 0; i < _instance.datas.Count; i++)
        {
            datas[i].Save();
        }
    }

    private void OnApplicationQuit()
    {
        AllSave();
    }
}