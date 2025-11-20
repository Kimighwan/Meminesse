using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : UIBase
{
    protected override void Start()
    {
        base.Start();

        //AudioManager.Instance.PlayBGM(BGM.ForestCradle);
    }

    protected override void Update()
    {
        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (UIManager.Instance.HasActivePopup) return;
        
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
        UIManager.Instance.OpenPopup<SettingsPopup>("SettingsPopup");
    }

    public void OnClickQuit()   
    {
        UIManager.Instance.OpenPopup<ExitPopUp>("ExitPopUp");
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }
}
