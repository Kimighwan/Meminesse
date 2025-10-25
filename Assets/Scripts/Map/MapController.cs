using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour
{
    [SerializeField] Map[] maps;
    [SerializeField] Map startingMap;
    [SerializeField] Map currentMap;
    [SerializeField] Transform respawnPoint;

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
        if (targetMap == null || targetMap == currentMap) return;

        currentMap.gameObject.SetActive(false);
        targetMap.gameObject.SetActive(true);
        currentMap = targetMap;
        currentMap.CheckMapVisited();
        player.transform.position = targetPoint.position;
        respawnPoint = targetPoint;
    }

    public IEnumerator RespawnPlayer(PlayerController player)
    {
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        player.transform.position = respawnPoint.position;
        player.gameObject.SetActive(true);
        yield return null;
    }
}