using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class FieldItems : MonoBehaviour
{
    public Item item;

    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody2D가 없다면 자동 추가
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        // 드랍 아이템은 중력 받도록 설정
        rb.gravityScale = 1f; // 중력 세기
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전은 막음
    }

    private void Start()
    { 
        Destroy(gameObject, 30f);
    }
    public void SetItem(Item _item)
    {
        item.itemName = _item.itemName;
        item.itemId = _item.itemId;
        item.itemImage = _item.itemImage;
        item.desc = _item.desc;
        item.itemImage = _item.itemImage;

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
        if (collision.CompareTag("Player"))
        {
            PickUpItem(collision.gameObject);
        }
    }

    private void PickUpItem(GameObject player)
    {
        //int itemId = item.itemId;
        InventoryUI.Instance.AddItemToInventory(item.itemId);

        Debug.Log($"플레이어가 {item.itemName} 획득!");
        Destroy(gameObject);
    }
}
