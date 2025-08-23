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

    // 종료하시겠습니까 - 예
    public void OnConfirmQuit()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  //Unity 에디터에서 실행 중일 때만 아래 코드를 실행
        UnityEditor.EditorApplication.isPlaying = false;       //에디터에서 Play 중지
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }

    // 종료하시겠습니까 - 아니오
    public void OnCancelQuit()
    {
        // 팝업 닫기
        exitConfirmationPopupUI.SetActive(false);

        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (exitConfirmationPopupUI.activeSelf) return;
    }

}
