using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : UIBase
{
    [SerializeField]
    private UIBase settingUI;

    [SerializeField]
    private UIBase exitConfirmationPopupUI;

    protected override void Start()
    {
        base.Start();

        AudioManager.Instance.PlayBGM(BGM.ForestCradle);
    }

    protected override void Update()
    {
        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (settingUI != null && settingUI.IsActive) return;
        
        base.Update();
    }

    public void OnClickNewGame()
    {
        Instantiate(Resources.Load<GameObject>("UI/LoadingCanvas"));
    }

    public void OnClickLoadGame()
    {
        //SceneManager.LoadScene("ReloadScene");  
    }

    public void OnClickSetting()
    {
        settingUI?.Show();
    }

    public void OnClickQuit()   
    {
        exitConfirmationPopupUI?.Show();
        //SetCurrentButton(exitConfirmationPopupUI.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }

    // 종료하시겠습니까 - 예
    public void OnConfirmQuit()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                 
        UnityEditor.EditorApplication.isPlaying = false;       
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }

    // 종료하시겠습니까 - 아니오
    public void OnCancelQuit()
    {
        exitConfirmationPopupUI?.Hide();
    }

}
