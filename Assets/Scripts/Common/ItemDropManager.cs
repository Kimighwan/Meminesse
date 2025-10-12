using UnityEngine;

public class ItemDropManager : SingletonBehaviour<ItemDropManager>
{
    [SerializeField] int dropItemCount;
    public GameObject fieldItemPrefab;
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MonsterDrop(Vector3 monsterPos)
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 dropPos = monsterPos + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 1f), 0);
            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);
        }
    }

    // 디버그용 
    public void TestItemDrop()
    {
        for (int i = 0; i < dropItemCount; i++)
        {
            Vector3 Pos = player.transform.position + new Vector3(3.0f, 0, 3.0f);
            Debug.Log("Player Pos : " + Pos);
            Vector3 dropPos = Pos + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 1f), 0);
            GameObject go = Instantiate(fieldItemPrefab, dropPos, Quaternion.identity);
        }
    }
}
