using UnityEngine;

public class PausePopUp : UIBase
{
    public void OnClickResume()
    {
        UIManager.Instance.CloseAllPopups();
    }
    public void OnClickSetting()
    {
        UIManager.Instance.OpenPopup<SettingsPopup>("SettingsPopup");
    }
    public void OnClickMain()
    {
        var popup = UIManager.Instance.OpenPopup<ExitPopUp>("ExitPopUp");
        HUD.Instance.DestroyHUD();
        popup.ShowMessage(ExitPopUp.ConfirmType.GoToMain);
    }

    public void OnClickQuit()
    {
        var popup = UIManager.Instance.OpenPopup<ExitPopUp>("ExitPopUp");
        popup.ShowMessage(ExitPopUp.ConfirmType.QuitGame);
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }
}
