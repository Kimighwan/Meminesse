using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] MapController mapController;
    public Transform targetPoint;
    public Map targetMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            mapController.TeleportPlayer(collision.GetComponent<PlayerController>(), targetMap, targetPoint);
        }
    }
}