using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : UIBase
{
    [SerializeField]
    private GameObject settingUI;


    [SerializeField]
    private GameObject exitConfirmationPopupUI;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (settingUI.activeSelf) return;
        
        base.Update();
        // UIBase에서 키보드 모드와 마우스 모드를 전환하는 로직이 있으므로, 여기서는 추가적인 로직이 필요하지 않음
    }

    public void OnClickNewGame()
    {
        
    }

    public void OnClickLoadGame()
    {
        SceneManager.LoadScene("ReloadScene");  
    }

    public void OnClickSetting()
    {
    }

    public void OnClickQuit()   
    {
        exitConfirmationPopupUI.SetActive(true);
        //SetCurrentButton(exitConfirmationPopupUI.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

    }


}
