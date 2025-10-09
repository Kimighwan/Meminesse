using UnityEditor;
using UnityEngine;

public class SaveLoadTool
{
    [MenuItem("Tools/Save&Load/AllSave")]
    public static void AllSave()
    {
        SaveFileDataManager.Instance.Save();
        PlayerDataManager.Instance.Save();
        ItemDataManager.Instance.Save();
        SettingDataManager.Instance.Save();
    }

    [MenuItem("Tools/Save&Load/AllLoad")]
    public static void AllLoad()
    {
        SaveFileDataManager.Instance.Load();
        PlayerDataManager.Instance.Load();
        ItemDataManager.Instance.Load();
        SettingDataManager.Instance.Load();
    }
}

