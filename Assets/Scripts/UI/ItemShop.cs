using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour
{
    public List<ItemData> itemDataList;

    // 돈 두종류 임시 이름
    [SerializeField]
    private TextMeshProUGUI mintMoney;
    [SerializeField]
    private TextMeshProUGUI redMoney;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 데이터 가져오기          <<<<<<<<<< 오류 재검토
        itemDataList = ItemDataManager.Instance.GetItemDataList();

        // 돈 초기화         
        ItemData dia, ma;
        if (ItemDataManager.Instance.ExistItem(21) != false)
        {
            dia = itemDataList.Find(item => item.itemId == 21);
            mintMoney.text = dia.count.ToString();
        }
        else
            mintMoney.text = "0";

        if (ItemDataManager.Instance.ExistItem(22) != false)
        {
            ma = itemDataList.Find(item => item.itemId == 22);
            redMoney.text = ma.count.ToString();
        }
        else
        {
            //Debug.Log("마연석 아이템이 존재하지 않음");
            redMoney.text = "0";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
