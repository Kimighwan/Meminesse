using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int mapId;
    [SerializeField] GameObject respawnPoint;

    public void CheckMapVisited()
    {
        SaveFileDataManager.Instance.AddVisitedMapId(mapId);
        Debug.Log("CheckMapVisited() 호출");
    }

    public int GetMapId()
    {
        return mapId;
    }

    public GameObject GetRespawnPoint()
    {
        return respawnPoint;
    }
}

