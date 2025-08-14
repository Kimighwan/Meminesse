using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private int mapId;

    private void OnEnable()
    {
        SaveFileDataManager.Instance.AddVisitedMapId(mapId);
    }
}

