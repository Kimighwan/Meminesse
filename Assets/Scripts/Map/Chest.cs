using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private int giveAmount = 1; // 상자에서 주는 스킬포인트 수량
    [SerializeField] private GameObject fieldItemPrefab;

    private SpriteRenderer sr;
    private bool isOpened = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpened) return;

        if (collision.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;

        sr.sprite = openSprite;

        DropSkillPoint(transform.position, giveAmount);

        Debug.Log($"보물상자 - 루네스 {giveAmount} 지급");
    }

    void DropSkillPoint(Vector3 dropPos, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = dropPos + new Vector3(
                Random.Range(-0.3f, 0.3f),
                0f,
                0
            );

            GameObject go = Instantiate(fieldItemPrefab, spawnPos, Quaternion.identity);

            ItemData data = DataTableManager.Instance.GetItemData("23");
            if (data == null) return;

            FieldItems field = go.GetComponent<FieldItems>();
            if (field != null)
            {
                field.SetItem(data, 1);
            }

            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = new Vector2(
                    Random.Range(-0.5f, 0.5f),
                    Random.Range(2.5f, 4.0f)
                );

                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }
}
