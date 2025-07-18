using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPopup : UIBase
{
    public GameObject settingsBackGround;
    public GameObject audioPopup;
    public GameObject graphicPopup;
    public GameObject controlPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        //EventSystem.current.SetSelectedGameObject(firstButton);   // Default -> first button is selected (audio button)
        settingsBackGround.SetActive(true); 
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //_popup.SetActive(false); // 팝업은 처음에 꺼져있어야함
        DontDestroyOnLoad(this); // 씬이 넘어가도 이 스크립트는 파괴되면 안됨
    }


    public void OnClickAudioButton()
    {
        audioPopup.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        audioPopup.SetActive(true);          //audio tab active, others inactive
        graphicPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    public void OnClickGraphicButton()
    {
        graphicPopup.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        graphicPopup.SetActive(true);       //graphic tab active, others inactive
        audioPopup.SetActive(false);
        controlPopup.SetActive(false);
    }

    public void OnClickControlButton()
    {
        controlPopup.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        controlPopup.SetActive(true);       //control tab active, others inactive
        graphicPopup.SetActive(false);
        audioPopup.SetActive(false);
    }

}
