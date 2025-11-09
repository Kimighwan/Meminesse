using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class ExitConfirmPopUp : UIBase
{
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TextMeshProUGUI message;
    
    ConfirmType currentType;
    Action onConfirmAction;

    private void Start()
    {
        message.text = "정말 게임을 종료하시겠습니까?";
        onConfirmAction = QuitGame;
    }

    public enum ConfirmType
    {
        QuitGame, //게임 종료
        GoToMain  //메인 메뉴로
    }
    protected override void Awake()
    {
        SetRootObject(popupRoot);
        base.Awake();
    }
    public override bool HandleEscape()
    {
        Debug.Log("ExitConfirmPopUp - ESC로 닫기 실행");
        Hide();

        return true;
    }

    public void ShowMessage(ConfirmType type)
    {
        currentType = type;

        switch (type)
        {
            case ConfirmType.QuitGame:
                message.text = "정말 게임을 종료하시겠습니까?";
                onConfirmAction = QuitGame;
                break;

            case ConfirmType.GoToMain:
                message.text = "메인 메뉴로 돌아가시겠습니까?";
                onConfirmAction = GoToMainMenu;
                break;
        }

        Debug.Log("ExitConfirmPopUp - 종료 확인 팝업");
        Show(); // UIBase의 Show() 호출 (SetActive(true))
    }
    public void OnClickYes()
    {
        onConfirmAction?.Invoke();  
    }

    // 종료하시겠습니까 - 예
    public void QuitGame()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                 
        UnityEditor.EditorApplication.isPlaying = false;     
#else
        Application.Quit();     
#endif
    }

    //메인메뉴로 가시겠습니까 - 예
    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // 종료하시겠습니까 - 아니오
    public void OnCancelQuit()
    {
        this.Hide();
    }

}
