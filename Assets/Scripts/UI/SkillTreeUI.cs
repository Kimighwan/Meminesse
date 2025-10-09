using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : UIBase
{
    /*------------
        Desc Ui
     ------------*/
    [SerializeField]
    private GameObject descActiveGO;
    [SerializeField]
    private Image descUISkill_Icon;
    [SerializeField]
    private TextMeshProUGUI descUISkillName;
    [SerializeField]
    private TextMeshProUGUI descUIText;
    private const string IMAGE_PATH = "UI/SkillTree/SkillIcon";

    public Image[] edgeImage; //= new Image[24];

    [SerializeField]
    private RectTransform contentRectTransform;
    [SerializeField]
    private AllSkillDescUI allSkillDescUI;
    [SerializeField]
    private Button skillActiveButton;
    private bool[] BActive = new bool[24];

    [Header("Skill descUI Position")]
    private Vector2 upPosition = Vector2.zero;
    private Vector2 downPosition = new Vector2(0f, -310f);

    [Header("Scroll position to move")]
    private Vector2 rightPosition = new Vector2(-800f, 0f);    // Scroll Right Position to move right
    private Vector2 leftPosition = new Vector2(-0f, 0f);     // Scroll Left Position to move right

    private float elapsedTime;
    private float clampT;
    private float t;

    // conditional variable
    public bool descUIActiveCheck = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();


        UpdateEdgeImage();

        if(Input.GetKeyDown(KeyCode.F)) // 스킬 활성화 버튼 클릭
        {
            skillActiveButton.onClick.Invoke();
        }
    }

    private void OnEnable()
    {
        descActiveGO.SetActive(false);
    }

    public void OnClickSkillNode(int nodeID)    // 스킬 활성화 버튼
    {
        descUIActiveCheck = true;
        descActiveGO.SetActive(true);
        // 활성화 버튼 이벤트 전부 삭제
        skillActiveButton.onClick.RemoveAllListeners();
        skillActiveButton.onClick.AddListener(() => BActive[nodeID] = true);    // 해당 노드 스킬 찍었음을 확인하는 조건 변수
        skillActiveButton.onClick.AddListener(() => skillActiveButton.gameObject.SetActive(false)); // 스킬 활성화 버튼 비활성화




        // 노드에 맞게 설명과 버튼 이벤트 할당
        // 1) 노드에 맞는 기능들 버튼에 부여
        // 2) 모든 능력치 보는 UI에 능력치 업데이트
        switch (nodeID)
        {
            case 1:
                descUIText.text = "체력 +1";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/AddHP");
                descUISkillName.text = "추가 체력";

                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.AddHP);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.AddHP);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.AddHP();
                //allSkillDescUI.AddHP();
                break;
            case 2:
            case 4:
            case 11:
                descUIText.text = "대쉬 쿨타임 감소 -0.5초";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/DashCoolTime");
                descUISkillName.text = "대쉬 쿨타임 감소";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DashCoolDownDecrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DashCoolTimeDecrease);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if(nodeID == 2 && !BActive[1])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 4 && !BActive[2])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 1 && !BActive[8])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.DashCoolTimeDecrease();
                //allSkillDescUI.DashCoolTimeDecrease();
                break;
            case 3:
            case 10:
                descUIText.text = "아이템 드랍률 상승 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Item");
                descUISkillName.text = "아이템 트랍률 상승";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.ItemDropRate);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.ItemDropRate);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 3 && !BActive[1])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 10 && !BActive[7])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.ItemDropRate();
                //allSkillDescUI.ItemDropRate();
                break;
            case 5:
            case 13:
                descUIText.text = "재화 드랍률 상승 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Gold");
                descUISkillName.text = "재화 드럅률 상승";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.GoldDropRate);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.GoldDropRate);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 5 && !BActive[3])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 13 && !BActive[10])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.GoldDropRate();
                //allSkillDescUI.GoldDropRate();
                break;
            case 6:
            case 14:
                descUIText.text = "방어력 무시 증가 +50%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/DefenseIgnore");
                descUISkillName.text = "방어력 무시";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DefenceIgnoreIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DefenseIgnoreIncrease);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 6 && !BActive[21])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 14 && !BActive[22])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.DefenseIgnoreIncrease();
                //allSkillDescUI.DefenseIgnoreIncrease();
                break;
            case 7:
            case 17:
            case 20:
                descUIText.text = "공격력 상승 +10%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/AddDamage");
                descUISkillName.text = "공격력 상승";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DamageIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DamageIncrease);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 7 && !BActive[6])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 17 && !BActive[14])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 20 && !BActive[17])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.DamageIncrease();
                //allSkillDescUI.DamageIncrease();
                break;
            case 8:
            case 15:
            case 18:
                descUIText.text = "스킬 쿨타임 감소 -10%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/SkillCoolTime");
                descUISkillName.text = "스킬 쿨타임 감소";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.SkillCoolDownDecrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.SkillCoolTimeDecrease);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 8 && !BActive[7])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 15 && !BActive[14])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 18 && !BActive[15])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.SkillCoolTimeDecrease();
                //allSkillDescUI.SkillCoolTimeDecrease();
                break;
            case 9:
            case 12:
            case 16:
            case 19:
                descUIText.text = "체력 회복량 증가 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/HpRecoveryAmount");
                descUISkillName.text = "회복량 증가";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.HealingProbabilityIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.HealingAmount);
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (nodeID == 9 && !BActive[7])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 12 && !BActive[9])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 16 && !BActive[14])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 19 && !BActive[16])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                //PlayerDataManager.Instance.HealingAmountIncrease();
                //allSkillDescUI.HealingAmount();
                break;
            case 21:
                descUIText.text = "첫 번째 상위 패시브";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active1");
                descUISkillName.text = "첫 번째 상위 패시브";
                if (!BActive[nodeID])
                {
                    //skillActiveButton.onClick.AddListener();
                    //skillActiveButton.onClick.AddListener();
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (!BActive[4] || !BActive[5])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                break;
            case 22:
                descUIText.text = "두 번째 상위 패시브";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active2");
                descUISkillName.text = "두 번째 상위 패시브";
                if (!BActive[nodeID])
                {
                    //skillActiveButton.onClick.AddListener();
                    //skillActiveButton.onClick.AddListener();
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (!BActive[11] || !BActive[12] || !BActive[13])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                break;
            case 23:
                descUIText.text = "세 번째 상위 패시브";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active3");
                descUISkillName.text = "세 번째 상위 패시브";
                if (!BActive[nodeID])
                {
                    //skillActiveButton.onClick.AddListener();
                    //skillActiveButton.onClick.AddListener();
                    skillActiveButton.gameObject.SetActive(true);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                if (!BActive[18] || !BActive[19] || !BActive[20])
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                break;
        }
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }

    public void MoveRightScroll()
    {
        if (contentRectTransform.anchoredPosition == rightPosition)
            return;

        StopAllCoroutines();
        StartCoroutine(MoveRight(contentRectTransform.anchoredPosition));
    }

    public void MoveLeftScroll()
    {
        if (contentRectTransform.anchoredPosition == leftPosition)
            return;

        StopAllCoroutines();
        StartCoroutine(MoveLeft(contentRectTransform.anchoredPosition));
    }

    private IEnumerator MoveRight(Vector2 startPos)
    {
        var startTime = Time.time;

        while(Time.time - startTime < 1)
        {
            contentRectTransform.anchoredPosition = Vector2.Lerp(startPos, rightPosition, (Time.time - startTime));
            yield return null;
        }

        contentRectTransform.anchoredPosition = rightPosition;
    }

    private IEnumerator MoveLeft(Vector2 startPos)
    {
        var startTime = Time.time;

        while (Time.time - startTime < 1)
        {
            contentRectTransform.anchoredPosition = Vector2.Lerp(startPos, leftPosition, (Time.time - startTime));
            yield return null;
        }

        contentRectTransform.anchoredPosition = leftPosition;
    }

    private void UpdateEdgeImage()
    {
        for(int i = 1; i <= 23; i++)
        {
            if (BActive[i])
            {
                edgeImage[i].sprite = Resources.Load<Sprite>($"UI/SkillTree/normal outline");
            }
        }
    }
}
