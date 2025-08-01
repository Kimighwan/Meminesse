using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPopup : UIBase
{
    public GameObject popUp;
    public GameObject audioPopup;
    public GameObject graphicPopup;
    public GameObject controlPopup;

    // 포커스가 상위부분에 있는지 하위부분에 있는지 판단하는 변수
    private static bool isTopFocus = true;

    [SerializeField]
    public TextMeshProUGUI currentBtnText;
    [SerializeField] 
    private TextMeshProUGUI[] topButtonTexts;

    private Color32 defaultColor = new Color32(170, 131, 167, 255);
    private Color32 selectedColor = Color.white;

    //오디오 슬라이더 관리
    private AudioMixer audioMixer;
    [SerializeField]
    private Slider BGMslider;
    [SerializeField]
    private Slider SFXslider;
    [SerializeField]
    private TextMeshProUGUI BGMText;
    [SerializeField]
    private TextMeshProUGUI SFXText;

    private void Awake()
    {
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); //UIBase꺼 그대로 사용 

        audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");

        currentButton = invisibleDummyButton; // 현재 선택된 버튼을 더미버튼으로 초기화

        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);

        BGMslider.onValueChanged.AddListener(OnBGMSliderEvent);
        SFXslider.onValueChanged.AddListener(OnSFXSliderEvent);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        ChangeTextColor();
        if (isTopFocus)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                isTopFocus = false;

            // esc - 설정창 끄기
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("설정 팝업 닫힘");
                this.popUp.SetActive(false); // 팝업 꺼짐
            }
        }

        // esc - 상위로 (오디오 버튼으로)
        else
        {
            //ChangeTextColor(currentBtnText);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventSystem.current.SetSelectedGameObject(currentButton);
                isTopFocus = true;
            }
        }

        //DontDestroyOnLoad(this); // 씬이 넘어가도 이 스크립트는 파괴되면 안됨
    }


    // 선택된 버튼 색상 흰색으로 바꾸는 함수
    public void ChangeTextColor()
    {
        foreach (var btnText in topButtonTexts)
        {
            if (btnText == currentBtnText)
                btnText.color = selectedColor;   // 선택된 버튼 흰색
            else
                btnText.color = defaultColor;   // 나머지 보라색
        }
    }

    public void OnClickAudioButton()
    {
        currentBtnText = topButtonTexts[0]; // 오디오 버튼 텍스트로 현재 버튼 텍스트 설정
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
        Debug.Log("오디오 설정창 활성화");
    }

    public void OnClickGraphicButton()
    {
        currentBtnText = topButtonTexts[1]; // 그래픽 버튼 텍스트로 현재 버튼 텍스트 설정
        graphicPopup.SetActive(true);       //graphic tab active, others inactive
        audioPopup.SetActive(false);
        controlPopup.SetActive(false);
        Debug.Log("그래픽 설정창 활성화");
    }

    public void OnClickControlButton()
    {
        currentBtnText = topButtonTexts[2]; // 조작법 버튼 텍스트로 현재 버튼 텍스트 설정
        controlPopup.SetActive(true);       //control tab active, others inactive
        graphicPopup.SetActive(false);
        audioPopup.SetActive(false);
        Debug.Log("조작법 설정창 활성화");
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
}
