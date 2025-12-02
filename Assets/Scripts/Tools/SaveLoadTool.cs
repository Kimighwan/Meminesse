#if UNITY_EDITOR
using UnityEditor;
public class SaveLoadTool
{
    [MenuItem("Tools/Save&Load/AllSave")]
    public static void AllSave()
    {
        SaveFileDataManager.Instance.Save();
        PlayerDataManager.Instance.Save();
        InventoryDataManager.Instance.Save();
        SettingDataManager.Instance.Save();
    }

    [MenuItem("Tools/Save&Load/AllLoad")]
    public static void AllLoad()
    {
        SaveFileDataManager.Instance.Load();
        PlayerDataManager.Instance.Load();
        InventoryDataManager.Instance.Load();
        SettingDataManager.Instance.Load();
    }
}
#endif

