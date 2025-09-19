using UnityEngine;

public class Portal : MapController
{
    public Transform targetPoint;
    public Map targetMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            TeleportPlayer(collision.GetComponent<PlayerController>(), targetMap, targetPoint);
        }
    }
}