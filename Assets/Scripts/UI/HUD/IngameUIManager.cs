using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 인게임 씬 관리 
public class IngameUIManager : MonoBehaviour
{ 
    [SerializeField] private UIBase pauseMenuPopUp;
    [SerializeField] private UIBase settingPopUp;

    [SerializeField] private ExitConfirmPopUp exitPopup;
    [SerializeField] private UIBase exitConfirmPopUp;   // 종료하시겠습니까? 

    [SerializeField] private UIBase skillTreeUI;
    [SerializeField] private UIBase inventoryUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance != null && UIManager.Instance.HasActivePopup)
            {
                return;
            }


            if (pauseMenuPopUp != null)
            {
                if (pauseMenuPopUp.IsActive)
                {
                    pauseMenuPopUp.Hide();
                    Debug.Log("IngameUIManager - 일시정지 팝업 끄기");
                }
                else
                {
                    pauseMenuPopUp.Show();
                    Debug.Log("IngameUIManager - 일시정지 팝업 열기");
                }
            }
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))
        {
            if (inventoryUI != null)
            {
                if (inventoryUI.IsActive)
                {
                    inventoryUI.Hide();
                }
                else
                {
                    inventoryUI.Show();
                }
            }
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("SkillTree")))
        {
            if (skillTreeUI != null)
            {
                if (skillTreeUI.IsActive)
                {
                    skillTreeUI.Hide();
                }
                else
                {
                    skillTreeUI.Show();
                }
            }
    }
}
    public void OnClickResume()
    {
        pauseMenuPopUp.Hide();
    }
    public void OnClickSetting()
    {
        settingPopUp.Show();
    }
    public void OnClickMain()
    { 
        exitPopup.ShowMessage(ExitConfirmPopUp.ConfirmType.GoToMain);
        exitConfirmPopUp.Show();
    }
    public void OnClickQuit()
    {
        exitPopup.ShowMessage(ExitConfirmPopUp.ConfirmType.QuitGame);
        exitConfirmPopUp.Show();
    }
}