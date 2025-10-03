using UnityEditor;
using UnityEngine;

public class SaveLoadTool
{
    [MenuItem("Tools/Save&Load/AllSave")]
    public static void AllSave()
    {
        DataManager.Instance.AllSave();
    }

    [MenuItem("Tools/Save&Load/AllLoad")]
    public static void AllLoad()
    {
        DataManager.Instance.AllLoad();
    }
}

