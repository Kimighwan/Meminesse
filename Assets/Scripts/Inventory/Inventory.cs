using System.Xml.Serialization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;

// 인벤토리창 모든 UI 관리
public class Inventory : UIBase
{
    // 데이터베이스
    public ItemDatabase itemDatabase;
    public List<ItemData> itemDataList;

    // 돈 두종류 임시 이름
    [SerializeField] private TextMeshProUGUI mintMoney;
    [SerializeField] private TextMeshProUGUI redMoney;

    // 플레이어가 들고있는 무기 이미지
    public Image[] weaponImages;

    // 무기 레벨 UI
    public GameObject[] weaponLevels; 

    public static Inventory Instance;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        itemDataList = ItemDataManager.Instance.GetItemDataList(); // 아이템 데이터 가져오기 
        RefreshInventory();
    }  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        base.Start();
    }

    void OnEnable()
    {
        Cursor.visible = true;
        itemDataList = ItemDataManager.Instance.GetItemDataList(); // 아이템 데이터 가져오기 
        RefreshInventory();
    }

    private void OnDisable()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // 인벤토리 모든 부분 최신화 
    public void RefreshInventory()
    {   
        UpdateMoney();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep());
        HpUIManager.Instance.UpdateHearts();
        InventoryUI.Instance.UpdateInventory();
        Debug.Log(">>>>>>>>>>>>>>> 인벤토리 갱신!");
    }

    public void UpdateMoney()
    {
        ItemData dia, ma; // 다이아, 마연석 
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

    //현재 무기레벨을 ui에 단계별 표시
    public void UpdateWeaponUI(int weaponStep)
    {
        int step = weaponStep;

        for (int i = 0; i < weaponLevels.Length; i++)
        {
            // 무기 레벨 칸 UI 업데이트
            if (i < step)
                weaponLevels[i].SetActive(true);
            else
                weaponLevels[i].SetActive(false); 

            // 사용자 초상화의 검 이미지 업데이트
            if (i == step - 1)
                weaponImages[i].gameObject.SetActive(true);
            else
                weaponImages[i].gameObject.SetActive(false);
            
        }
    }


    // 무기 업그레이드 - 다른 곳으로 이동시키자 
    public void UpgradeWeaponStep()
    {
        // 마연석 개수에 따라 조건문 걸 부분
        PlayerDataManager.Instance.UpgradeWeaponStep();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep());
        InventoryItemDescription.Instance.ShowWeaponDescription(); // 무기 업그레이드 후 설명창 업데이트
    }
}
