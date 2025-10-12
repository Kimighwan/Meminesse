using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour
{
    [SerializeField] Map[] maps;
    [SerializeField] Map startingMap;
    [SerializeField] Map currentMap;

    private void Start()
    {
        foreach (var map in maps)
        {
            map.gameObject.SetActive(false);
        }

        if (startingMap != null)
        {
            startingMap.gameObject.SetActive(true);
            currentMap = startingMap;
            currentMap.CheckMapVisited();
        }
    }

    public void TeleportPlayer(PlayerController player, Map targetMap, Transform targetPoint)
    {
        if (targetMap == null || targetPoint == null || player == null)
        {
            return;
        }

        currentMap.gameObject.SetActive(false);
        targetMap.gameObject.SetActive(true);
        currentMap = targetMap;
        currentMap.CheckMapVisited();

        player.transform.position = targetPoint.position;
    }

    public IEnumerator RespawnPlayer(PlayerController player)
    {
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        player.transform.position = currentMap.GetRespawnPoint().transform.position;
        player.gameObject.SetActive(true);
        yield return null;
    }
}