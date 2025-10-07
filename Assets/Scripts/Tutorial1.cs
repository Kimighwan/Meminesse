using UnityEngine;

public class Tutorial1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Scene에 ItemDataManager을 넣어야합니다?
    // 파란포션 2개 노란포션 1개 추가
    void AddPotion()
    {
        ItemDataManager.Instance.AddItem(31, 2);
        ItemDataManager.Instance.AddItem(32, 1);
        InventoryUI.Instance.UpdateInventory();
    }
}
