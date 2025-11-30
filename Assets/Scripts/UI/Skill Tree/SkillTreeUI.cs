using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : UIBase
{
    [Header("Desc UI")]
    [SerializeField] GameObject descActiveGO;
    [SerializeField] Image descUISkill_Icon;
    [SerializeField] TextMeshProUGUI descUISkillName;
    [SerializeField] TextMeshProUGUI descUIText;
    [SerializeField] GameObject confirmUI;
    [SerializeField] TextMeshProUGUI skillPointText;
    const string IMAGE_PATH = "UI/SkillTree/SkillIcon";

    public Image[] edgeImage;
    public Image[] skillImage;

    [SerializeField] RectTransform contentRectTransform;
    [SerializeField] AllSkillDescUI allSkillDescUI;
    [SerializeField] Button skillActiveButton;

    [Header("Scroll position to move")]
    Vector2 rightPosition = new Vector2(-800f, 0f);    // Scroll Right Position to move right
    Vector2 leftPosition = new Vector2(-0f, 0f);     // Scroll Left Position to move right

    public bool descUIActiveCheck = false;

    private void OnEnable()
    {
        skillPointText.text = InventoryDataManager.Instance.GetItemCountById("23").ToString();
        descUIText.gameObject.SetActive(false);
        descUISkill_Icon.gameObject.SetActive(false);
        descUISkillName.gameObject.SetActive(false);
        skillActiveButton.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        UpdateEdgeImage();
    }

    public override void OnShown()
    {
        base.OnShown();
        descActiveGO.SetActive(false);
        skillActiveButton.gameObject.SetActive(false);
    }

    public void OnClickSkillNode(int nodeID)    // 스킬 활성화 버튼
    {
        descUIActiveCheck = true;
        skillActiveButton.gameObject.SetActive(true);
        descActiveGO.SetActive(true);
        descUISkill_Icon.gameObject.SetActive(true);

        skillActiveButton.onClick.RemoveAllListeners();

        bool skillPointCheck = InventoryDataManager.Instance.ExistItem("23");

        // TODO : 스킬 포인트 확인
        if (!skillPointCheck)
        {
            skillActiveButton.onClick.AddListener(() => confirmUI.SetActive(true));
        }
        else
        {
            InventoryDataManager.Instance.ItemCountReduce("23", 1);
            skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.SetSkillActive(nodeID));    // 해당 노드 스킬 찍었음을 확인하는 조건 변수
            skillActiveButton.onClick.AddListener(() => skillActiveButton.gameObject.SetActive(false)); // 스킬 활성화 버튼 비활성화
            skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.SetSkillActive(nodeID));  
        }
        

        // 설명 UI 활성화
        descUIText.gameObject.SetActive(true);
        descUISkill_Icon.gameObject.SetActive(true);
        descUISkillName.gameObject.SetActive(true);
        skillActiveButton.gameObject.SetActive(true);

        // 노드에 맞게 설명과 버튼 이벤트 할당
        // 1) 노드에 맞는 기능들 버튼에 부여
        // 2) 모든 능력치 보는 UI에 능력치 업데이트
        switch (nodeID)
        {
            case 1:
                descUIText.text = "체력 +1";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/AddHP");
                descUISkillName.text = "추가 체력";

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.AddHP);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                break;
            case 2:
            case 4:
            case 11:
                descUIText.text = "대쉬 쿨타임 감소 -0.5초";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/DashCoolTime");
                descUISkillName.text = "대쉬 쿨타임 감소";

                if (nodeID == 2 && !PlayerDataManager.Instance.GetSkillActive(1))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 4 && !PlayerDataManager.Instance.GetSkillActive(2))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 1 && !PlayerDataManager.Instance.GetSkillActive(8))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DashCoolDownDecrease);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 3:
            case 10:
                descUIText.text = "아이템 드랍률 상승 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Item");
                descUISkillName.text = "아이템 트랍률 상승";

                if (nodeID == 3 && !PlayerDataManager.Instance.GetSkillActive(1))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 10 && !PlayerDataManager.Instance.GetSkillActive(7))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.ItemDropRate);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 5:
            case 13:
                descUIText.text = "재화 드랍률 상승 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Gold");
                descUISkillName.text = "재화 드럅률 상승";

                if (nodeID == 5 && !PlayerDataManager.Instance.GetSkillActive(3))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 13 && !PlayerDataManager.Instance.GetSkillActive(10))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.GoldDropRate);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 6:
            case 14:
                descUIText.text = "방어력 무시 증가 +50%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/DefenseIgnore");
                descUISkillName.text = "방어력 무시";

                if (nodeID == 6 && !PlayerDataManager.Instance.GetSkillActive(21))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 14 && !PlayerDataManager.Instance.GetSkillActive(22))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DefenceIgnoreIncrease);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 7:
            case 17:
            case 20:
                descUIText.text = "공격력 상승 +10%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/AddDamage");
                descUISkillName.text = "공격력 상승";

                if (nodeID == 7 && !PlayerDataManager.Instance.GetSkillActive(6))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 17 && !PlayerDataManager.Instance.GetSkillActive(14))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 20 && !PlayerDataManager.Instance.GetSkillActive(17))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DamageIncrease);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 8:
            case 15:
            case 18:
                descUIText.text = "스킬 쿨타임 감소 -10%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/SkillCoolTime");
                descUISkillName.text = "스킬 쿨타임 감소";

                if (nodeID == 8 && !PlayerDataManager.Instance.GetSkillActive(7))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 15 && !PlayerDataManager.Instance.GetSkillActive(14))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 18 && !PlayerDataManager.Instance.GetSkillActive(15))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.SkillCoolDownDecrease);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 9:
            case 12:
            case 16:
            case 19:
                descUIText.text = "체력 회복량 증가 +25%";
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/HpRecoveryAmount");
                descUISkillName.text = "회복량 증가";

                if (nodeID == 9 && !PlayerDataManager.Instance.GetSkillActive(7))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 12 && !PlayerDataManager.Instance.GetSkillActive(9))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 16 && !PlayerDataManager.Instance.GetSkillActive(14))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }
                else if (nodeID == 19 && !PlayerDataManager.Instance.GetSkillActive(16))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.HealingProbabilityIncrease);
                    skillActiveButton.onClick.AddListener(() => PlayerDataManager.Instance.Save());
                }
                else
                    skillActiveButton.gameObject.SetActive(false);

                break;
            case 21:
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active1");
                descUISkillName.text = "첫 번째 상위 패시브";

                if (!PlayerDataManager.Instance.GetSkillActive(4) || !PlayerDataManager.Instance.GetSkillActive(5))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    descUIText.text = "첫 번째 상위 패시브";
                    skillActiveButton.onClick.AddListener(() => OpenTopSkillUI(1));
                }
                else
                {
                    skillActiveButton.gameObject.SetActive(false);
                    int index = PlayerDataManager.Instance.GetTopPassiveOfNode(1);
                    if(index == 1)
                        descUIText.text = "고수 모드";
                    else if(index == 2)
                        descUIText.text = "보통 모드";
                    else
                        descUIText.text = "겁쟁이 모드";
                }
                    
                break;
            case 22:
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active2");
                descUISkillName.text = "두 번째 상위 패시브";

                if (!PlayerDataManager.Instance.GetSkillActive(11) || !PlayerDataManager.Instance.GetSkillActive(12) || !PlayerDataManager.Instance.GetSkillActive(13))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    descUIText.text = "두 번째 상위 패시브";
                    skillActiveButton.onClick.AddListener(() => OpenTopSkillUI(2));
                }
                else
                {
                    skillActiveButton.gameObject.SetActive(false);
                    int index = PlayerDataManager.Instance.GetTopPassiveOfNode(2);
                    if (index == 1)
                        descUIText.text = "고수 모드";
                    else if (index == 2)
                        descUIText.text = "보통 모드";
                    else
                        descUIText.text = "겁쟁이 모드";
                }

                break;
            case 23:
                descUISkill_Icon.sprite = Resources.Load<Sprite>($"{IMAGE_PATH}/Active3");
                descUISkillName.text = "세 번째 상위 패시브";

                if (!PlayerDataManager.Instance.GetSkillActive(18) || !PlayerDataManager.Instance.GetSkillActive(19) || !PlayerDataManager.Instance.GetSkillActive(20))
                {
                    skillActiveButton.gameObject.SetActive(false);
                }

                if (!PlayerDataManager.Instance.GetSkillActive(nodeID))
                {
                    if (!skillPointCheck)
                        return;

                    descUIText.text = "세 번째 상위 패시브";
                    skillActiveButton.onClick.AddListener(() => OpenTopSkillUI(3));
                }
                else
                {
                    skillActiveButton.gameObject.SetActive(false);
                    int index = PlayerDataManager.Instance.GetTopPassiveOfNode(3);
                    if (index == 1)
                        descUIText.text = "고수 모드";
                    else if (index == 2)
                        descUIText.text = "보통 모드";
                    else
                        descUIText.text = "겁쟁이 모드";
                }

                break;
        }
    }
    
    #region Move Scroll
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
    #endregion

    void UpdateEdgeImage()
    {
        for(int i = 1; i <= 23; i++)
        {
            if (PlayerDataManager.Instance.GetSkillActive(i))
            {
                edgeImage[i].sprite = Resources.Load<Sprite>($"UI/SkillTree/normal outline");
                skillImage[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    void OpenTopSkillUI(int n)
    {
        var prefab = Resources.Load<GameObject>("PopUp/TopSkillUI");
        var instance = Instantiate(prefab);
        if (instance.TryGetComponent<TopSkillUI>(out TopSkillUI topSkillUI))
        {
            topSkillUI.Init(n);
        }
    }
}
