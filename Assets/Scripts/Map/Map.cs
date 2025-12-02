using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Setting Map")]
    [SerializeField] private int mapId;
    [SerializeField] GameObject respawnPoint;
    [SerializeField] GameObject[] portals;

    [Header("Setting Enemy Point")]
    [SerializeField] GameObject[] enemySpawnPoints;


    public void CheckMapVisited()
    {
        SaveFileDataManager.Instance.AddVisitedMapId(mapId);
        SpawnEnemy();
    }

    public int GetMapId()
    {
        return mapId;
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point.gameObject;
    }

    public GameObject GetRespawnPoint()
    {
        return respawnPoint;
    }

    void SpawnEnemy()
    {

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            var newGO = Instantiate(Resources.Load<GameObject>($"Enemy/{enemySpawnPoints[i].name}")
                , enemySpawnPoints[i].transform.position, Quaternion.identity);

            MapController.Instance.activeObjects.Add(newGO);
        }
    }
}

