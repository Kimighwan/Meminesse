using UnityEngine;

public class Trap : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.Damaged(20, null, false);
            MapController.Instance.RelocatePlayer(player);
        }
        else if(collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Entity>();
            enemy.Damaged(1000, transform.position);
        }
    }
}