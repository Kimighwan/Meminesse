using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeUI : UIBase
{
    [SerializeField]
    private RectTransform descUIRectTransform;
    [SerializeField]
    private TextMeshProUGUI descUIText;
    [SerializeField]
    private RectTransform contentRectTransform;
    [SerializeField]
    private AllSkillDescUI allSkillDescUI;
    [SerializeField]
    private Button skillActiveButton;
    private bool[] BActive = new bool[20];

    [Header("Skill descUI Position")]
    private Vector2 upPosition = Vector2.zero;
    private Vector2 downPosition = new Vector2(0f, -310f);

    [Header("Scroll position to move")]
    private Vector2 rightPosition = new Vector2(-1400f, 292f);    // Scroll Right Position to move right
    private Vector2 leftPosition = new Vector2(-0f, 292f);     // Scroll Left Position to move right

    private float elapsedTime;
    private float clampT;
    private float t;

    // conditional variable
    public bool descUIActiveCheck = false;
    public bool up = false;
    public bool down = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (up)
        {
            elapsedTime += Time.deltaTime;
            clampT = Mathf.Clamp01(elapsedTime);
            t = Mathf.SmoothStep(0f, 1f, clampT);
            descUIRectTransform.anchoredPosition = Vector2.Lerp(downPosition, upPosition, t);

            if(t >= 1f)
            {
                up = false;
                descUIRectTransform.anchoredPosition = upPosition;
                elapsedTime = 0;
            }
        }

        if(down)
        {
            elapsedTime += Time.deltaTime;
            clampT = Mathf.Clamp01(elapsedTime);
            t = Mathf.SmoothStep(0f, 1f, clampT);
            descUIRectTransform.anchoredPosition = Vector2.Lerp(upPosition, downPosition, t);

            if (t >= 1f)
            {
                down = false;
                descUIRectTransform.anchoredPosition = downPosition;
                elapsedTime = 0;
            }
        }

    }

    public void OnClickSkillNode(int nodeID)
    {
        if (down)
            return;

        if (!descUIActiveCheck)
            up = true;

        descUIActiveCheck = true;
        // 활성화 버튼 이벤트 전부 삭제
        skillActiveButton.onClick.RemoveAllListeners();
        skillActiveButton.onClick.AddListener(() => BActive[nodeID] = true);
        skillActiveButton.onClick.AddListener(() => skillActiveButton.gameObject.SetActive(false));

        // 노드에 맞게 설명과 버튼 이벤트 할당
        switch (nodeID)
        {
            case 1:
                descUIText.text = "체력 +1";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.AddHP);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.AddHP);
                    skillActiveButton.gameObject.SetActive(true);
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
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DashCoolTimeDecrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DashCoolTimeDecrease);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.DashCoolTimeDecrease();
                //allSkillDescUI.DashCoolTimeDecrease();
                break;
            case 3:
            case 10:
                descUIText.text = "아이템 드랍률 상승 +25%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.ItemDropRate);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.ItemDropRate);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.ItemDropRate();
                //allSkillDescUI.ItemDropRate();
                break;
            case 5:
            case 13:
                descUIText.text = "재화 드랍률 상승 +25%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.GoldDropRate);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.GoldDropRate);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.GoldDropRate();
                //allSkillDescUI.GoldDropRate();
                break;
            case 6:
            case 14:
                descUIText.text = "방어력 무시 증가 +50%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DefenseIgnoreIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DefenseIgnoreIncrease);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.DefenseIgnoreIncrease();
                //allSkillDescUI.DefenseIgnoreIncrease();
                break;
            case 7:
            case 17:
            case 20:
                descUIText.text = "공격력 상승 +10%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.DamageIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.DamageIncrease);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.DamageIncrease();
                //allSkillDescUI.DamageIncrease();
                break;
            case 8:
            case 15:
            case 18:
                descUIText.text = "스킬 쿨타임 감소 -10%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.SkillCoolTimeDecrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.SkillCoolTimeDecrease);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.SkillCoolTimeDecrease();
                //allSkillDescUI.SkillCoolTimeDecrease();
                break;
            case 9:
            case 12:
            case 16:
            case 19:
                descUIText.text = "체력 회복량 증가 +25%";
                if (!BActive[nodeID])
                {
                    skillActiveButton.onClick.AddListener(PlayerDataManager.Instance.HealingAmountIncrease);
                    skillActiveButton.onClick.AddListener(allSkillDescUI.HealingAmount);
                    skillActiveButton.gameObject.SetActive(true);
                }
                else
                    skillActiveButton.gameObject.SetActive(false);
                //PlayerDataManager.Instance.HealingAmountIncrease();
                //allSkillDescUI.HealingAmount();
                break;
            case 21:
                descUIText.text = "첫 번째 상위 패시브";
                break;
            case 22:
                descUIText.text = "두 번째 상위 패시브";
                break;
            case 23:
                descUIText.text = "세 번째 상위 패시브";
                break;

        }
        PlayerDataManager.Instance.Save();
    }

    public void DownDescUIAnmation()
    {
        if (down || up)
            return;

        descUIActiveCheck = false;
        down = true;
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


    /*--------------------
     * apply skill data
     -------------------*/

    //public void AddHP()
    //{
    //    PlayerDataManager.Instance.AddHP();
    //}

    //public void HealingAmount()
    //{
    //    PlayerDataManager.Instance.HealingAmountIncrease();
    //}

    //public void DashCoolTimeDecrease()
    //{
    //    PlayerDataManager.Instance.DashCoolTimeDecrease();
    //}

    //public void SkillCoolTimeDecrease()
    //{
    //    PlayerDataManager.Instance.SkillCoolTimeDecrease();
    //}

    //public void ItemDropRate()
    //{
    //    PlayerDataManager.Instance.ItemDropRate();
    //}

    //public void GoldDropRate()
    //{
    //    PlayerDataManager.Instance.GoldDropRate();
    //}

    //public void DefenseIgnoreIncrease()
    //{
    //    PlayerDataManager.Instance.DefenseIgnoreIncrease();
    //}

    //public void DamageIncrease()
    //{
    //    PlayerDataManager.Instance.DamageIncrease();
    //}
}
