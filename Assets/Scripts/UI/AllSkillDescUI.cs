using System;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllSkillDescUI : MonoBehaviour
{
    public void AddHP()
    {
        if(!BAcitveAddHp)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            hpImg.color = new Color(hpImg.color.r, hpImg.color.g, hpImg.color.b, 1);
            hpTxt.color = new Color(hpTxt.color.r, hpTxt.color.g, hpTxt.color.b, 1);
            hpValue.text = "+1";
            hpValue.color = new Color(hpTxt.color.r, hpTxt.color.g, hpTxt.color.b, 1);
        }
        // 능력치 수치만큼 더 추가 하기
        // 추가 체력은 노드가 1개 뿐이라 더 이상 추가될 상황이 없다.
    }

    public void HealingAmount()
    {
        if (!BAcitveHealingAmount)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            addHealingAmountImg.color = new Color(addHealingAmountImg.color.r, addHealingAmountImg.color.g, addHealingAmountImg.color.b, 1);
            addHealingAmountTxt.color = new Color(addHealingAmountTxt.color.r, addHealingAmountTxt.color.g, addHealingAmountTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        addHealingAmountValue.color = new Color(addHealingAmountTxt.color.r, addHealingAmountTxt.color.g, addHealingAmountTxt.color.b, 1);
        addHealingAmountValue.text = (DataManager.Player.GetAdditionalHealingProbability() * 100).ToString() + "%";
    }

    public void DashCoolTimeDecrease()
    {
        if (!BAcitveDashCoolTimeDecrease)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            dashCoolTimeDecreaseImg.color = new Color(dashCoolTimeDecreaseImg.color.r, dashCoolTimeDecreaseImg.color.g, dashCoolTimeDecreaseImg.color.b, 1);
            dashCoolTimeDecreaseTxt.color = new Color(dashCoolTimeDecreaseTxt.color.r, dashCoolTimeDecreaseTxt.color.g, dashCoolTimeDecreaseTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        dashCoolTimeDecreaseValue.color = new Color(dashCoolTimeDecreaseTxt.color.r, dashCoolTimeDecreaseTxt.color.g, dashCoolTimeDecreaseTxt.color.b, 1);
        dashCoolTimeDecreaseValue.text = (DataManager.Player.GetDashCoolDown() * 100).ToString() + "%";
    }

    public void SkillCoolTimeDecrease()
    {
        if (!BAcitveSkillCoolTimeDecrease)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            skillCoolTimeDecreaseImg.color = new Color(skillCoolTimeDecreaseImg.color.r, skillCoolTimeDecreaseImg.color.g, hpImg.color.b, 1);
            skillCoolTimeDecreaseTxt.color = new Color(skillCoolTimeDecreaseTxt.color.r, skillCoolTimeDecreaseTxt.color.g, skillCoolTimeDecreaseTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        skillCoolTimeDecreaseValue.color = new Color(skillCoolTimeDecreaseTxt.color.r, skillCoolTimeDecreaseTxt.color.g, skillCoolTimeDecreaseTxt.color.b, 1);
        skillCoolTimeDecreaseValue.text = (DataManager.Player.GetSkillCoolDown() * 100).ToString() + "%";
    }

    public void ItemDropRate()
    {
        if (!BAcitveItemDropRate)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            itemDropRateImg.color = new Color(itemDropRateImg.color.r, itemDropRateImg.color.g, itemDropRateImg.color.b, 1);
            itemDropRateTxt.color = new Color(itemDropRateTxt.color.r, itemDropRateTxt.color.g, itemDropRateTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        itemDropRateValue.color = new Color(itemDropRateTxt.color.r, itemDropRateTxt.color.g, itemDropRateTxt.color.b, 1);
        itemDropRateValue.text = (DataManager.Player.GetItemDropRate() * 100).ToString() + "%";
    }

    public void GoldDropRate()
    {
        if (!BAcitveGoldDropRate)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            goldDropRateImg.color = new Color(goldDropRateImg.color.r, goldDropRateImg.color.g, goldDropRateImg.color.b, 1);
            goldDropRateTxt.color = new Color(goldDropRateTxt.color.r, goldDropRateTxt.color.g, goldDropRateTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        goldDropRateValue.color = new Color(goldDropRateTxt.color.r, goldDropRateTxt.color.g, goldDropRateTxt.color.b, 1);
        goldDropRateValue.text = (DataManager.Player.GetGoldDropRate() * 100).ToString() + "%";
    }

    public void DefenseIgnoreIncrease()
    {
        if (!BAcitveDefenseIgnoreIncrease)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            defenseIgnoreIncreaseImg.color = new Color(defenseIgnoreIncreaseImg.color.r, defenseIgnoreIncreaseImg.color.g, defenseIgnoreIncreaseImg.color.b, 1);
            defenseIgnoreIncreaseTxt.color = new Color(defenseIgnoreIncreaseTxt.color.r, defenseIgnoreIncreaseTxt.color.g, defenseIgnoreIncreaseTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        defenseIgnoreIncreaseValue.color = new Color(defenseIgnoreIncreaseTxt.color.r, defenseIgnoreIncreaseTxt.color.g, defenseIgnoreIncreaseTxt.color.b, 1);
        defenseIgnoreIncreaseValue.text = (DataManager.Player.GetDefenseIgnore() * 100).ToString() + "%";
    }

    public void DamageIncrease()
    {
        if (!BAcitveAddDamage)
        {
            BAcitveAddHp = true;
            // 활성화 시키기
            addDamageImg.color = new Color(addDamageImg.color.r, addDamageImg.color.g, addDamageImg.color.b, 1);
            addDamageTxt.color = new Color(addDamageTxt.color.r, addDamageTxt.color.g, addDamageTxt.color.b, 1);
        }

        // 능력치 수치만큼 더 추가 하기
        addDamageValue.color = new Color(addDamageTxt.color.r, addDamageTxt.color.g, addDamageTxt.color.b, 1);
        addDamageValue.text = (DataManager.Player.GetAddDamage() * 100).ToString() + "%";
    }


    private bool BAcitveAddHp = false;
    private bool BAcitveHealingAmount = false;
    private bool BAcitveAddDamage = false;
    private bool BAcitveDefenseIgnoreIncrease = false;
    private bool BAcitveDashCoolTimeDecrease = false;
    private bool BAcitveSkillCoolTimeDecrease = false;
    private bool BAcitveItemDropRate = false;
    private bool BAcitveGoldDropRate = false;

    [Header("image")]
    [SerializeField]
    private Image hpImg;
    [SerializeField]
    private Image addHealingAmountImg;
    [SerializeField]
    private Image addDamageImg;
    [SerializeField]
    private Image defenseIgnoreIncreaseImg;
    [SerializeField]
    private Image dashCoolTimeDecreaseImg;
    [SerializeField]
    private Image skillCoolTimeDecreaseImg;
    [SerializeField]
    private Image itemDropRateImg;
    [SerializeField]
    private Image goldDropRateImg;
    [Header("text")]
    [SerializeField]
    private TextMeshProUGUI hpTxt;
    [SerializeField]
    private TextMeshProUGUI addHealingAmountTxt;
    [SerializeField]
    private TextMeshProUGUI addDamageTxt;
    [SerializeField]
    private TextMeshProUGUI defenseIgnoreIncreaseTxt;
    [SerializeField]
    private TextMeshProUGUI dashCoolTimeDecreaseTxt;
    [SerializeField]
    private TextMeshProUGUI skillCoolTimeDecreaseTxt;
    [SerializeField]
    private TextMeshProUGUI itemDropRateTxt;
    [SerializeField]
    private TextMeshProUGUI goldDropRateTxt;
    [Header("value")]
    [SerializeField]
    private TextMeshProUGUI hpValue;
    [SerializeField]
    private TextMeshProUGUI addHealingAmountValue;
    [SerializeField]
    private TextMeshProUGUI addDamageValue;
    [SerializeField]
    private TextMeshProUGUI defenseIgnoreIncreaseValue;
    [SerializeField]
    private TextMeshProUGUI dashCoolTimeDecreaseValue;
    [SerializeField]
    private TextMeshProUGUI skillCoolTimeDecreaseValue;
    [SerializeField]
    private TextMeshProUGUI itemDropRateValue;
    [SerializeField]
    private TextMeshProUGUI goldDropRateValue;
}
