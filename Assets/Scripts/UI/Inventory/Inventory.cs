using System.Xml.Serialization;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

// 인벤토리창 모든 UI 관리
public class Inventory : MonoBehaviour
{
    // 데이터베이스
    public ItemDatabase itemDatabase;

    // 돈 두종류 임시 이름
    [SerializeField]
    private TextMeshProUGUI mintMoney;
    [SerializeField]
    private TextMeshProUGUI redMoney;

    // 플레이어가 들고있는 무기 이미지
    public Image[] weaponImages;

    // 무기 레벨 UI
    public GameObject[] weaponLevels; 

    #region Singleton
    public static Inventory Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 돈 초기화
        Item dia, ma;
        dia = itemDatabase.itemDB.Find(item => item.itemId == 21);
        ma = itemDatabase.itemDB.Find(item => item.itemId == 22);
        mintMoney.text = dia.count.ToString();
        redMoney.text = ma.count.ToString();

        // 무기 레벨 UI 초기화
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep()); 


    }

    // Update is called once per frame
    void Update()
    {
        
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
        // 마연석 개수에 따라 조건문 걸 부분
        PlayerDataManager.Instance.UpgradeWeaponStep();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep());
        InventoryItemDescription.Instance.ShowWeaponDescription(); // 무기 업그레이드 후 설명창 업데이트
    }
}
