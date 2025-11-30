using UnityEngine;

public class ItemDrop : SingletonBehaviour<ItemDrop>
{
    [SerializeField] int dropItemCount = 3;
    public GameObject fieldItemPrefab;
    [SerializeField] private GameObject player;

    private readonly string[] possibleItemIDs =
    {
        "21", "22", "23",
        "31", "32", "33", "34", "35", "36", "37",
        "41", "42", "43", "44",
        "51", "52", "53"
    };

    public void DropRandomItem()
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 basePos = player.transform.position + new Vector3(3.0f, 0, 3.0f);
            Vector3 dropPos = basePos + new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 1f),
                0
            );

            string randomID = possibleItemIDs[UnityEngine.Random.Range(0, possibleItemIDs.Length)];

            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);

            ItemData data = DataTableManager.Instance.GetItemData(randomID);
            if (data == null) return;

            FieldItems field = go.GetComponent<FieldItems>();
            if (field != null)
            {
                field.SetItem(data, 1);
            }
        }
    }

    public void DropSkillPoint(Vector3 dropPos, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = dropPos + new Vector3(
                UnityEngine.Random.Range(-0.3f, 0.3f),
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
                    UnityEngine.Random.Range(-0.5f, 0.5f),
                    UnityEngine.Random.Range(2.5f, 4.0f)
                );

                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

}
