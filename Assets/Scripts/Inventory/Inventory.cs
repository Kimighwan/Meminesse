using System.Xml.Serialization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections;
using static UnityEditor.Timeline.Actions.MenuPriority;

// 인벤토리창 모든 UI 관리
public class Inventory : UIBase
{
    #region singleton
    public static Inventory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    public List<InventoryData> itemDataList;

    // 돈 두종류 임시 이름
    [SerializeField] private TextMeshProUGUI mintMoney;
    [SerializeField] private TextMeshProUGUI redMoney;

    // 플레이어가 들고있는 무기 이미지
    public Image[] weaponImages;

    // 무기 레벨 UI
    public GameObject[] weaponLevels;

    //체력바
    public HpUI inventoryHpBar;

    // 재화 불충분 경고 메시지
    [SerializeField] private TextMeshProUGUI message;

    private void OnEnable()
    {
        RefreshInventory();
    }

    // 인벤토리 모든 부분 최신화 
    public void RefreshInventory()
    {
        itemDataList = InventoryDataManager.Instance.GetItemDataList();

        UpdateMoney();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponLevel());
        inventoryHpBar.UpdateHearts();
        InventorySlots.Instance.UpdateInventory();
        Debug.Log(">>>>>>>>>>>>>>> 인벤토리 갱신!");
    }

    public void UpdateMoney()
    {
        if (InventoryDataManager.Instance.ExistItem("21") != false)
        {
            var inventoryData = InventoryDataManager.Instance.GetItemCountById("21");
            mintMoney.text = inventoryData.ToString();
        }
        else
            mintMoney.text = "0";

        if (InventoryDataManager.Instance.ExistItem("22") != false)
        {
            var inventoryData = InventoryDataManager.Instance.GetItemCountById("22");
            redMoney.text = inventoryData.ToString();
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
        int yyy = weaponStep;

        for (int i = 0; i < weaponLevels.Length; i++)
        {
            // 무기 레벨 칸 UI 업데이트
            if (i < yyy)
                weaponLevels[i].SetActive(true);
            else
                weaponLevels[i].SetActive(false); 

            // 사용자 초상화의 검 이미지 업데이트
            if (i == yyy - 1)
                weaponImages[i].gameObject.SetActive(true);
            else
                weaponImages[i].gameObject.SetActive(false);
        }
    }

    // 무기 업그레이드
    public void UpgradeWeaponStep()
    {
        int money = InventoryDataManager.Instance.GetItemCountById("22"); // 사용자 마연석 소지개수
        int weaponStep = PlayerDataManager.Instance.GetWeaponLevel();

        UpgradeData upgradeInfo = DataTableManager.Instance.GetUpgradeData("1" + weaponStep.ToString());
        if(upgradeInfo == null)
        {
            Debug.Log("업그레이드 데이터 null");
            return;
        }
        Debug.Log($"현재 무기 레벨: {weaponStep}, 보유 마연석: {money}, 필요 마연석 : {upgradeInfo.Count}");
        if (money <= upgradeInfo.Count)
        {
            Debug.Log($"업그레이드 실패! 마연석이 부족합니다. 필요: {upgradeInfo.Count}, 보유: {money}");

            message.gameObject.SetActive(true);
            return;
        }
        InventoryDataManager.Instance.ItemCountReduce("22", upgradeInfo.Count);
        message.gameObject.SetActive(false);
        PlayerDataManager.Instance.UpgradeWeaponLevel();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponLevel());
        InventoryItemDescription.Instance.ShowWeaponDescription(); // 무기 업그레이드 후 설명창 업데이트
        UpdateMoney();
        Debug.Log($"업그레이드 성공! 무기 레벨이 {weaponStep + 1}로 상승했습니다.");
    }
   
    // 디버그용 돈 추가 
    public void DebugAddMoney()
    {
        var itemData = DataTableManager.Instance.GetItemData("22");

        InventoryDataManager.Instance.AddItem(itemData, 1000);
        UpdateMoney();
    }

}
