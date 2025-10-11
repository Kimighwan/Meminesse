using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour
{
    // 돈 두종류 임시 이름
    [SerializeField]
    private TextMeshProUGUI mintMoney;
    [SerializeField]
    private TextMeshProUGUI redMoney;

    void Start()
    {
        // 돈 초기화
        if (InventoryDataManager.Instance.ExistItem("21") != false)
        {
            mintMoney.text = InventoryDataManager.Instance.GetItemCountById("21").ToString();
        }
        else
            mintMoney.text = "0";

        if (InventoryDataManager.Instance.ExistItem("22") != false)
        {
            redMoney.text = InventoryDataManager.Instance.GetItemCountById("22").ToString();
        }
        else
        {
            //Debug.Log("마연석 아이템이 존재하지 않음");
            redMoney.text = "0";
        }
    }
}
