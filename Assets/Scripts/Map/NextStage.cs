using UnityEngine;

public class NextStage : MonoBehaviour
{
    public Transform DestinationPoint;
    public Map DestinationMap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.position = DestinationPoint.position;
            DestinationMap.gameObject.SetActive(true);
            DestinationMap.CheckMapVisited();
            transform.parent.gameObject.SetActive(false);
        }
    }
}
