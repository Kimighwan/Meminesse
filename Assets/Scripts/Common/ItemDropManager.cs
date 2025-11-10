using UnityEngine;

public class ItemDropManager : SingletonBehaviour<ItemDropManager>
{
    [SerializeField] int dropItemCount = 3;
    public GameObject fieldItemPrefab;
    [SerializeField] private GameObject player;

    public void MonsterDrop(Vector3 monsterPos)
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 dropPos = monsterPos + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 1f), 0);
            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);
        }
    }

    private readonly string[] possibleItemIDs =
    {
        "21", "22", "23",
        "31", "32", "33", "34", "35", "36", "37",
        "41", "42", "43", "44",
        "51", "52", "53"
    };
    // 디버그용 
    public void TestItemDrop()
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            // 플레이어 근처 랜덤 위치
            Vector3 basePos = player.transform.position + new Vector3(3.0f, 0, 3.0f);
            Vector3 dropPos = basePos + new Vector3(
                UnityEngine.Random.Range(-0.5f, 0.5f),
                UnityEngine.Random.Range(-0.5f, 1f),
                0
            );

            // 프리팹 생성
            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);

            // 랜덤 아이템 ID 선택
            string randomItemID = possibleItemIDs[UnityEngine.Random.Range(0, possibleItemIDs.Length)];

            // DataTableManager에서 아이템 데이터 불러오기
            ItemData randomItemData = DataTableManager.Instance.GetItemData(randomItemID);

            if (randomItemData == null)
            {
                Debug.LogWarning($"[ItemDropManager] 아이템 ID {randomItemID} 데이터를 찾을 수 없음");
                continue;
            }

            // FieldItems에 데이터 전달
            FieldItems fieldItem = go.GetComponent<FieldItems>();
            if (fieldItem != null)
            {
                fieldItem.SetItem(randomItemData, 1); // 드랍 개수 1개 기본
                Debug.Log($"[ItemDropManager] 아이템 드랍됨: {randomItemData.name} (ID: {randomItemData.itemId})");
            }
            else
            {
                Debug.LogError("[ItemDropManager] FieldItems 컴포넌트가 프리팹에 없음");
            }
        }
    }
}
