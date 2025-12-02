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
            Debug.Log("Starting map activated");
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
        respawnPoint = targetPoint;
    }

    public IEnumerator RespawnPlayer(PlayerController player)
    {
        Debug.Log("Respawning player...");
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.enabled = false;

        yield return new WaitForSeconds(1f);

        playerSpriteRenderer.enabled = true;
        player.transform.position = respawnPoint.position;

        Debug.Log("Player position set to respawn point: " + respawnPoint.position);

        yield return null;
    }
}