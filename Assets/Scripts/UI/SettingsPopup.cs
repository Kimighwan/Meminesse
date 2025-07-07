using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsPopup : UIBase
{
    public GameObject audioPopup;
    public GameObject graphicPopup;
    public GameObject controlPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //EventSystem.current.SetSelectedGameObject(firstButton);   // Default -> first button is selected (audio button)
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //_popup.SetActive(false); // 팝업은 처음에 꺼져있어야함
        DontDestroyOnLoad(this); // 씬이 넘어가도 이 스크립트는 파괴되면 안됨
    }


    public void OnClickAudioButton()
    {
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    public void OnClickGraphicButton()
    {
        graphicPopup.SetActive(true);       //graphic tab active, others inactive
        audioPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    public void OnClickControlButton()
    {
        controlPopup.SetActive(true);       //control tab active, others inactive
        graphicPopup.SetActive(false);
        audioPopup.SetActive(false);
    }

}
