using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


public enum ConfirmType
{
    QuitGame, //게임 종료
    GoToMain  //메인 메뉴로
}

public class ExitPopUp : UIBase
{
    [SerializeField] private TextMeshProUGUI message;
    
    ConfirmType currentType;
    Action onConfirmAction;

    private bool initialized = false;


    public override void OnShown()
    {
        base.OnShown();

        if (!initialized)
        {
            initialized = true;
            ShowMessage(ConfirmType.QuitGame);
        }
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
    }
    public void OnClickYes()
    {
        onConfirmAction?.Invoke();  
    }

    // 종료하시겠습니까 - 예
    public void QuitGame()
    {
#if UNITY_EDITOR                 
        UnityEditor.EditorApplication.isPlaying = false;     
#else
        Application.Quit();     
#endif
    }

    //메인메뉴로 가시겠습니까 - 예
    private void GoToMainMenu()
    {
        UIManager.Instance.CloseAllPopups();
        SceneManager.LoadScene("LobbyScene");
    }

    // 종료하시겠습니까 - 아니오
    public void OnCancelQuit()
    {
        UIManager.Instance.ClosePopup(this);
    }

}
