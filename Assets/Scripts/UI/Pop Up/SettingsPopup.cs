using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;

[System.Serializable]
public class SettingPage
{
    public GameObject topButtons;
    public TextMeshProUGUI topButtonTexts;  // 설정창 상위 버튼 텍스트들
    public GameObject tabs;
}

public class SettingsPopup : UIBase
{
    [FormerlySerializedAs("popUp")]
    [SerializeField] private GameObject popupRoot;

    [SerializeField] private List<SettingPage> page = new List<SettingPage>();

    // 포커스가 상위부분에 있는지 하위부분에 있는지 판단하는 변수
    private static bool isTopFocus = true;

    [SerializeField]
    public TextMeshProUGUI currentBtnText;

    private Color32 defaultColor = new Color32(170, 131, 167, 255);
    private Color32 selectedColor = Color.white;

    //오디오 슬라이더 관리
    private AudioMixer audioMixer;
    [SerializeField] private Slider BGMslider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private TextMeshProUGUI BGMText;
    [SerializeField] private TextMeshProUGUI SFXText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); 

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        currentButton = invisibleDummyButton; // 현재 선택된 버튼을 더미버튼으로 초기화

        ShowTab(0);

        BGMslider.onValueChanged.AddListener(OnBGMSliderEvent);
        SFXslider.onValueChanged.AddListener(OnSFXSliderEvent);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!IsActive)
        {
            return;
        }
        ChangeTextColor();
        if (isTopFocus)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                isTopFocus = false;
        }
    }

    void OnEnable()
    {
        base.OnShown();
        isTopFocus = true;
    }

    public override bool HandleEscape()
    {
        if (!isTopFocus)
        {
            if (EventSystem.current != null && currentButton != null)
            {
                EventSystem.current.SetSelectedGameObject(currentButton);
            }

            isTopFocus = true;
            return true;
        }

        return base.HandleEscape();
    }

    // 선택된 버튼 색상 흰색으로 바꾸는 함수
    public void ChangeTextColor()
    {
        foreach (SettingPage p in page)
        {
            if (p.topButtonTexts == currentBtnText)
                p.topButtonTexts.color = selectedColor;   // 선택된 버튼 흰색
            else
                p.topButtonTexts.color = defaultColor;   // 나머지 보라색
        }
    }

    public void OnClickAudioButton()
    {
        ShowTab(0);
        Debug.Log("오디오 설정창 활성화");
    }

    public void OnClickGraphicButton()
    {
        ShowTab(1);
        Debug.Log("그래픽 설정창 활성화");
    }

    public void OnClickControlButton()
    {
        ShowTab(2);
        Debug.Log("조작법 설정창 활성화");
    }

    public void ShowTab(int index)
    {
        currentBtnText = page[index].topButtonTexts;
        for (int i = 0; i < page.Count; i++)
        {
            page[i].tabs.SetActive(false);
        }
        page[index].tabs.SetActive(true);
        SetCurrentButton(page[index].topButtons);
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }

    //오디오 슬라이더 관리
    public void OnBGMSliderEvent(float value)
    {
        BGMText.text = $"{(value * 100):F1}%";
    }

    public void OnSFXSliderEvent(float value)
    {
        SFXText.text = $"{(value * 100):F1}%";
    }

    // 오디오 슬라이더 조절시 음량 조절
    public void SetBGMSlider(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20);
    }

    public void SetSFXSlider(float value)
    { 
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
    }

    public void OnClickCloseButton()
    {
        UIManager.Instance.CloseTopPopup();
    }
}
