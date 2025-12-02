using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform targetPoint;
    public Map targetMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            MapController.Instance.TeleportPlayer(collision.GetComponent<PlayerController>(), targetMap, targetPoint);
            targetMap.SetRespawnPoint(targetPoint);
        }
    }
}