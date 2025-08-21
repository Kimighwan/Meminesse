using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    private int mapId;

    public void CheckMapVisited()
    {
        SaveFileDataManager.Instance.AddVisitedMapId(mapId);
        Debug.Log("CheckMapVisited() 호출");
    }
}

