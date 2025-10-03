using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private int mapId;
    [SerializeField] GameObject respawnPoint;

    public void CheckMapVisited()
    {
        //SaveFileDataManager.Instance.AddVisitedMapId(mapId);
        Debug.Log("CheckMapVisited() called for Map ID: " + mapId);
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