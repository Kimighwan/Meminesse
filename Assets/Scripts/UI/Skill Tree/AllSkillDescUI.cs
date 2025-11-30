using System;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AllSkillDescUI : MonoBehaviour
{
    private void OnEnable()
    {
        AddHP();
        HealingAmount();
        DashCoolTimeDecrease();
        SkillCoolTimeDecrease();
        ItemDropRate();
        GoldDropRate();
        DefenseIgnoreIncrease();
        DamageIncrease();
    }

    void AddHP()
    {
        if (PlayerDataManager.Instance.GetSkillActive(1))
        {
            // 활성화 시키기
            hpImg.color = new Color(hpImg.color.r, hpImg.color.g, hpImg.color.b, 1);
            hpTxt.color = new Color(hpTxt.color.r, hpTxt.color.g, hpTxt.color.b, 1);
            hpValue.text = "+1";
            hpValue.color = new Color(hpTxt.color.r, hpTxt.color.g, hpTxt.color.b, 1);
        }
        // 능력치 수치만큼 더 추가 하기
        // 추가 체력은 노드가 1개 뿐이라 더 이상 추가될 상황이 없다.
    }

    void HealingAmount()
    {
        if (PlayerDataManager.Instance.GetSkillActive(9))
        {
            // 활성화 시키기
            addHealingAmountImg.color = new Color(addHealingAmountImg.color.r, addHealingAmountImg.color.g, addHealingAmountImg.color.b, 1);
            addHealingAmountTxt.color = new Color(addHealingAmountTxt.color.r, addHealingAmountTxt.color.g, addHealingAmountTxt.color.b, 1);
            addHealingAmountValue.color = new Color(addHealingAmountTxt.color.r, addHealingAmountTxt.color.g, addHealingAmountTxt.color.b, 1);
            addHealingAmountValue.text = "+" + (PlayerDataManager.Instance.GetAdditionalHealingProbability() * 100).ToString() + "%";
        }
    }

    void DashCoolTimeDecrease()
    {
        if (PlayerDataManager.Instance.GetSkillActive(2))
        {
            // 활성화 시키기
            dashCoolTimeDecreaseImg.color = new Color(dashCoolTimeDecreaseImg.color.r, dashCoolTimeDecreaseImg.color.g, dashCoolTimeDecreaseImg.color.b, 1);
            dashCoolTimeDecreaseTxt.color = new Color(dashCoolTimeDecreaseTxt.color.r, dashCoolTimeDecreaseTxt.color.g, dashCoolTimeDecreaseTxt.color.b, 1);

            dashCoolTimeDecreaseValue.color = new Color(dashCoolTimeDecreaseTxt.color.r, dashCoolTimeDecreaseTxt.color.g, dashCoolTimeDecreaseTxt.color.b, 1);
            dashCoolTimeDecreaseValue.text = "-" + (PlayerDataManager.Instance.GetDashCoolDown()).ToString() + "/s";
        }
    }

    void SkillCoolTimeDecrease()
    {
        if (PlayerDataManager.Instance.GetSkillActive(8))
        {
            // 활성화 시키기
            skillCoolTimeDecreaseImg.color = new Color(skillCoolTimeDecreaseImg.color.r, skillCoolTimeDecreaseImg.color.g, hpImg.color.b, 1);
            skillCoolTimeDecreaseTxt.color = new Color(skillCoolTimeDecreaseTxt.color.r, skillCoolTimeDecreaseTxt.color.g, skillCoolTimeDecreaseTxt.color.b, 1);

            skillCoolTimeDecreaseValue.color = new Color(skillCoolTimeDecreaseTxt.color.r, skillCoolTimeDecreaseTxt.color.g, skillCoolTimeDecreaseTxt.color.b, 1);
            skillCoolTimeDecreaseValue.text = (PlayerDataManager.Instance.GetSkillCoolDown() * 100).ToString() + "%";
        }
    }

    void ItemDropRate()
    {
        if (PlayerDataManager.Instance.GetSkillActive(3))
        {
            // 활성화 시키기
            itemDropRateImg.color = new Color(itemDropRateImg.color.r, itemDropRateImg.color.g, itemDropRateImg.color.b, 1);
            itemDropRateTxt.color = new Color(itemDropRateTxt.color.r, itemDropRateTxt.color.g, itemDropRateTxt.color.b, 1);
            itemDropRateValue.color = new Color(itemDropRateTxt.color.r, itemDropRateTxt.color.g, itemDropRateTxt.color.b, 1);
            itemDropRateValue.text = "+" + (PlayerDataManager.Instance.GetItemDropRate()).ToString() + "%";
        }
    }

    void GoldDropRate()
    {
        if (PlayerDataManager.Instance.GetSkillActive(5))
        {
            // 활성화 시키기
            goldDropRateImg.color = new Color(goldDropRateImg.color.r, goldDropRateImg.color.g, goldDropRateImg.color.b, 1);
            goldDropRateTxt.color = new Color(goldDropRateTxt.color.r, goldDropRateTxt.color.g, goldDropRateTxt.color.b, 1);
            goldDropRateValue.color = new Color(goldDropRateTxt.color.r, goldDropRateTxt.color.g, goldDropRateTxt.color.b, 1);
            goldDropRateValue.text = "+" + (PlayerDataManager.Instance.GetGoldDropRate()).ToString() + "%";
        }
    }

    void DefenseIgnoreIncrease()
    {
        if (PlayerDataManager.Instance.GetSkillActive(6))
        {
            // 활성화 시키기
            defenseIgnoreIncreaseImg.color = new Color(defenseIgnoreIncreaseImg.color.r, defenseIgnoreIncreaseImg.color.g, defenseIgnoreIncreaseImg.color.b, 1);
            defenseIgnoreIncreaseTxt.color = new Color(defenseIgnoreIncreaseTxt.color.r, defenseIgnoreIncreaseTxt.color.g, defenseIgnoreIncreaseTxt.color.b, 1);

            defenseIgnoreIncreaseValue.color = new Color(defenseIgnoreIncreaseTxt.color.r, defenseIgnoreIncreaseTxt.color.g, defenseIgnoreIncreaseTxt.color.b, 1);
            defenseIgnoreIncreaseValue.text = "+" + (PlayerDataManager.Instance.GetDefenseIgnore()).ToString() + "%";
        }
    }

    void DamageIncrease()
    {
        if (PlayerDataManager.Instance.GetSkillActive(7))
        {
            // 활성화 시키기
            addDamageImg.color = new Color(addDamageImg.color.r, addDamageImg.color.g, addDamageImg.color.b, 1);
            addDamageTxt.color = new Color(addDamageTxt.color.r, addDamageTxt.color.g, addDamageTxt.color.b, 1);
            addDamageValue.color = new Color(addDamageTxt.color.r, addDamageTxt.color.g, addDamageTxt.color.b, 1);
            addDamageValue.text = (PlayerDataManager.Instance.GetAddDamage() * 100).ToString() + "% 증가";
        }
    }

    [Header("image")]
    [SerializeField] Image hpImg;
    [SerializeField] Image addHealingAmountImg;
    [SerializeField] Image addDamageImg;
    [SerializeField] Image defenseIgnoreIncreaseImg;
    [SerializeField] Image dashCoolTimeDecreaseImg;
    [SerializeField] Image skillCoolTimeDecreaseImg;
    [SerializeField] Image itemDropRateImg;
    [SerializeField] Image goldDropRateImg;

    [Header("text")]
    [SerializeField] TextMeshProUGUI hpTxt;
    [SerializeField] TextMeshProUGUI addHealingAmountTxt;
    [SerializeField] TextMeshProUGUI addDamageTxt;
    [SerializeField] TextMeshProUGUI defenseIgnoreIncreaseTxt;
    [SerializeField] TextMeshProUGUI dashCoolTimeDecreaseTxt;
    [SerializeField] TextMeshProUGUI skillCoolTimeDecreaseTxt;
    [SerializeField] TextMeshProUGUI itemDropRateTxt;
    [SerializeField] TextMeshProUGUI goldDropRateTxt;

    [Header("value")]
    [SerializeField] TextMeshProUGUI hpValue;
    [SerializeField] TextMeshProUGUI addHealingAmountValue;
    [SerializeField] TextMeshProUGUI addDamageValue;
    [SerializeField] TextMeshProUGUI defenseIgnoreIncreaseValue;
    [SerializeField] TextMeshProUGUI dashCoolTimeDecreaseValue;
    [SerializeField] TextMeshProUGUI skillCoolTimeDecreaseValue;
    [SerializeField] TextMeshProUGUI itemDropRateValue;
    [SerializeField] TextMeshProUGUI goldDropRateValue;
}
