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
            // TODO: Trap에 부딪히면 피가 깍이고 리스폰된다.
        }

    }
}
