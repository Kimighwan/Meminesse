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

    public void OneWayPlatfrom()
    {
        if(currentPlatform != null)
            StartCoroutine(DisableCollider());
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
        var platformCollider = currentPlatform.GetComponent<PlatformEffector2D>();

        platformCollider.rotationalOffset = 180f;
        yield return new WaitForSeconds(0.25f);
        platformCollider.rotationalOffset = 0f;
    }
}
