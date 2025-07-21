using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPopup : UIBase
{
    public GameObject popUp;
    public GameObject audioPopup;
    public GameObject graphicPopup;
    public GameObject controlPopup;

    //오디오 슬라이더 관리
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
        currentButton = invisibleDummyButton; // 현재 선택된 버튼을 더미버튼으로 초기화

        //EventSystem.current.SetSelectedGameObject(firstButton);   // Default -> first button is selected (audio button)
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("설정 팝업 닫힘");
            this.popUp.SetActive(false); // 팝업 꺼짐
        }
        DontDestroyOnLoad(this); // 씬이 넘어가도 이 스크립트는 파괴되면 안됨
    }


    public void OnClickAudioButton()
    {
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
        Debug.Log("오디오 설정창 활성화");


    }

    public void OnClickGraphicButton()
    {
        //graphicPopup.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        graphicPopup.SetActive(true);       //graphic tab active, others inactive
        audioPopup.SetActive(false);
        controlPopup.SetActive(false);
        Debug.Log("그래픽 설정창 활성화");
    }

    public void OnClickControlButton()
    {
        controlPopup.SetActive(true);       //control tab active, others inactive
        graphicPopup.SetActive(false);
        audioPopup.SetActive(false);
        Debug.Log("조작법 설정창 활성화");
    }

    protected override void SetCurrentButton(GameObject gb)
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

}
