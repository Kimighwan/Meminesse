using UnityEngine;

public class Trap : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{GetType()} : 충돌");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Damaged(20, null, false);
            MapController.Instance.RelocatePlayer(player);
        }
    }
}