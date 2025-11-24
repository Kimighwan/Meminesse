using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    GameObject currentPlatform;
    Collider2D playerCollider;

    void Start()
    {
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) && Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisableCollider());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = collision.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentPlatform = null;
        }
    }

    IEnumerator DisableCollider()
    {
        Collider2D platformCollider = currentPlatform.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return null;
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
