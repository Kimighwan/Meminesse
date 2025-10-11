using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FieldItems : MonoBehaviour
{
    public ItemData itemData;

    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    bool dontDestroy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 1f; // 중력 세기
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전은 막음

        SetItem();
    }

    private void Start()
    {
        StartCoroutine(BlinkBeforeDestroy(55f, 5f));
    }

    public void SetItem()
    {
        string resultItemId = Random.Range(21, 23).ToString();

        itemData = DataTableManager.Instance.GetItemData(resultItemId);
        spriteRenderer.sprite = Resources.Load<Sprite>($"Item/{itemData.itemId}");

        rb.AddForce(new Vector2(Random.Range(-1f, 1f), 3f), ForceMode2D.Impulse);
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fieldItem이 플레이어와 충돌했을 때 
        if (collision.CompareTag("Player"))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        if (itemData == null)
        {
            return;
        }

        Destroy(this.gameObject);

        InventoryDataManager.Instance.AddItem(itemData, 1);
        Debug.Log($"플레이어가 {itemData.name} 획득!");
        
    }

    // 깜빡거리는 효과 
    private IEnumerator BlinkBeforeDestroy(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        float endTime = Time.time + duration;
        bool visible = true;

        while (Time.time < endTime)
        {
            visible = !visible;
            if (spriteRenderer != null)
                spriteRenderer.enabled = visible;

            yield return new WaitForSeconds(0.2f); 
        }

        if(!dontDestroy)
            Destroy(gameObject);
    }
}
