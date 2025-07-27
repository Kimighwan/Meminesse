using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : UIBase
{
    [SerializeField]
    private GameObject settingUI;
    //[SerializeField]
    //private GameObject exitConfirmationUI;
    public GameObject exitConfirmationPopup;

    protected override void Start()
    {
        base.Start();

        // 커서 기본 설정
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        // 포커스를 명시적으로 줘야 UI가 키보드 입력을 받음
        //EventSystem.current.SetSelectedGameObject(invisibleDummyButton);

        // 또한 currentButton도 초기화 필요
        //SetCurrentButton(invisibleDummyButton);
    }

    protected override void Update()
    {
        if (settingUI.activeSelf) return;
        if (exitConfirmationPopup.activeSelf) return;
        base.Update();
        // UIBase에서 키보드 모드와 마우스 모드를 전환하는 로직이 있으므로, 여기서는 추가적인 로직이 필요하지 않음
    }

    public void OnClickNewGame()
    {
        
    }

    public void OnClickLoadGame()
    {
        SceneManager.LoadScene("ReloadScene");   // 세이브 파일 불러오기 씬으로 이동
    }

    public void OnClickSetting()
    {
    }

    public void OnClickQuit()     //Quit button
    {
        // 종료 재확인 팝업 창
        exitConfirmationPopup.SetActive(true);
        SetCurrentButton(exitConfirmationPopup.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

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
        exitConfirmationPopup.SetActive(false);
    }


}
