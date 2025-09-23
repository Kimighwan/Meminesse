using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FieldItems : MonoBehaviour
{
    public Item item;

    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 1f; // 중력 세기
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전은 막음
    }

    private void Start()
    {
        Destroy(gameObject, 60f);
        StartCoroutine(BlinkBeforeDestroy(55f, 5f));
    }

    public void SetItem(Item _item)
    {
        item = new Item
        {
            itemId = _item.itemId,
            itemName = _item.itemName,
            itemImage = _item.itemImage,
            desc = _item.desc
        };

        if (spriteRenderer != null && item.itemImage != null)
        {
            spriteRenderer.sprite = item.itemImage;
        }

        rb.AddForce(new Vector2(Random.Range(-1f, 1f), 3f), ForceMode2D.Impulse);
    }

    public Item GetItem()
    {
        return item;
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
        if (item == null)
        {
            return;
        }

        Destroy(this.gameObject);

        InventoryUI.Instance.AddItemToInventory(item.itemId);
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.UpdateInventory();
            Inventory.Instance.UpdateMoney();
        }
        Debug.Log($"플레이어가 {item.itemName} 획득!");
        
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
    }
}
