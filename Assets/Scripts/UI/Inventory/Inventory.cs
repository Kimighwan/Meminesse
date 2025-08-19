using System.Xml.Serialization;
using TMPro;
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

    // 무기 레벨 UI
    public GameObject[] weaponImages; 

    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        int hp = weaponStep;

        for (int i = 0; i < weaponImages.Length; i++)
        {
            if (i < hp)
                weaponImages[i].SetActive(true); // 보이기
            else
                weaponImages[i].SetActive(false); // 숨기기
        }
        // Debug.Log("현재 무기 레벨 : " + weaponStep); //임시 확인용
    }

    // 무기 업그레이드
    public void UpgradeWeaponStep()
    {
        PlayerDataManager.Instance.UpgradeWeaponStep();
        UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep());
    }
}
