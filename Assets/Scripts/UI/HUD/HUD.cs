using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : SingletonBehaviour<HUD>
{
    [SerializeField] private HpUI hpBar;

    [Header("CoolTime Images")]
    [SerializeField] Image dashImage;
    [SerializeField] Image holySlashImage;
    [SerializeField] Image lightCut;

    [Header("Cool Time")]
    [SerializeField] float holySlashCoolTime = 30;
    [SerializeField] float lightCutCoolTime = 15;

    [Header("Cool Time Text")]
    [SerializeField] TMPro.TextMeshProUGUI dashCoolTimeText;
    [SerializeField] TMPro.TextMeshProUGUI holySlashCoolTimeText;
    [SerializeField] TMPro.TextMeshProUGUI lightCutCoolTimeText;

    float holySlashMaxTime;
    float lightCutMaxTime;

    void Start()
    {
        hpBar.UpdateHearts();

        holySlashMaxTime = holySlashCoolTime * (1 - PlayerDataManager.Instance.GetSkillCoolDown()) + 0.2f;
        lightCutMaxTime = lightCutCoolTime * (1 - PlayerDataManager.Instance.GetSkillCoolDown()) + 0.4f;

        dashCoolTimeText.enabled = false;
        holySlashCoolTimeText.enabled = false;
        lightCutCoolTimeText.enabled = false;
    }

    #region Cool Time Image
    public void StartDashCoolTime() => StartCoroutine(DashCoolTimeImageUpdate());
    public void StartHolySlashCoolTime() => StartCoroutine(HolySlashCoolTimeImageUpdate());
    public void StartLightCutCoolTime() => StartCoroutine(LightCutCoolTimeImageUpdate());

    IEnumerator DashCoolTimeImageUpdate()
    {
        float coolTime = 0;
        dashCoolTimeText.enabled = true;

        while (coolTime < PlayerDataManager.Instance.GetDashCoolDown() + 0.3f)
        {
            coolTime += Time.deltaTime;
            dashImage.fillAmount = coolTime / (PlayerDataManager.Instance.GetDashCoolDown() + 0.3f);
            dashCoolTimeText.text = string.Format("{0:N1}", (PlayerDataManager.Instance.GetDashCoolDown() + 0.3f) - coolTime);
            yield return null;
        }
        dashCoolTimeText.enabled = false;
    }

    IEnumerator HolySlashCoolTimeImageUpdate()
    {
        float coolTime = 0;
        holySlashCoolTimeText.enabled = true;

        while (coolTime < holySlashMaxTime)
        {
            coolTime += Time.deltaTime;
            holySlashImage.fillAmount = coolTime / holySlashMaxTime;
            holySlashCoolTimeText.text = string.Format("{0:N1}", holySlashMaxTime - coolTime);
            yield return null;
        }

        holySlashCoolTimeText.enabled = false;
    }

    IEnumerator LightCutCoolTimeImageUpdate()
    {
        float coolTime = 0;
        lightCutCoolTimeText.enabled = true;

        while (coolTime < lightCutMaxTime)
        {
            coolTime += Time.deltaTime;
            lightCut.fillAmount = coolTime / lightCutMaxTime;
            lightCutCoolTimeText.text = string.Format("{0:N1}", lightCutMaxTime - coolTime);
            yield return null;
        }

        lightCutCoolTimeText.enabled = false;
    }

    #endregion

    public void UpdateHUD() => hpBar.UpdateHearts();
}
