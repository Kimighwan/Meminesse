using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    private List<ISaveAndLoad> datas = new List<ISaveAndLoad>();

    PlayerDataManager _player;
    SaveFileDataManager _saveFile;
    ItemDataManager _item;
    SettingDataManager _setting;

    public static DataManager Instance { get { return _instance; } }
    public static PlayerDataManager Player { get { return _instance._player; } }
    public static SaveFileDataManager SaveFile { get { return _instance._saveFile; } }
    public static ItemDataManager Item { get { return _instance._item; } }
    public static SettingDataManager Setting { get { return Instance._setting; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            _player = new PlayerDataManager();
            _saveFile = new SaveFileDataManager();
            _item = new ItemDataManager();
            _setting = new SettingDataManager();

            _instance.datas.Add(Player);
            _instance.datas.Add(Item);
            _instance.datas.Add(Setting);
            _instance.datas.Add(SaveFile);
        }
        else
        {
            Destroy(gameObject);
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