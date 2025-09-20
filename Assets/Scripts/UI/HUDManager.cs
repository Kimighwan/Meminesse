using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// HUD만 관리하는게 아니므로 스크립트 이름을 수정해야 합니다
// 인게임 씬 관리 
public class HUDManager : UIBase
{
    [SerializeField] private GameObject pauseMenuPopUp;
    [SerializeField] private GameObject settingPopUp;

    [SerializeField] private GameObject GoToMainConfirmationPopupUI; // 진행중인 게임을 종료하고 메인 메뉴로 나가시겠습니까?
    [SerializeField] private GameObject exitConfirmationPopupUI;   // 종료하시겠습니까? 

    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject inventoryUI;

    private bool isPopUpOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    new void Update()
    {
        HandlePopup();
    }
    public void HandlePopup()
    {
        if (pauseMenuPopUp.activeSelf || inventoryUI.activeSelf || skillTreeUI.activeSelf)
        {
            Cursor.visible = true;
            isPopUpOpen = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = pauseMenuPopUp.activeSelf;
            if (settingPopUp.activeSelf)
            {
                settingPopUp.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (GoToMainConfirmationPopupUI.activeSelf) // 진행중인 게임을 종료하고 메인 메뉴로 나가시겠습니까?
            {
                GoToMainConfirmationPopupUI.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (exitConfirmationPopupUI.activeSelf) // 종료하시겠습니까? 
            {
                exitConfirmationPopupUI.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (skillTreeUI.activeSelf)
            {
                skillTreeUI.SetActive(false);
                isPopUpOpen = false;
                Cursor.visible = false;
            }
            else if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                isPopUpOpen = false;
                Cursor.visible = false;
            }
            else
                pauseMenuPopUp.SetActive(!pauseMenuPopUp.activeSelf);

            if (pauseMenuPopUp.activeSelf)
            {
                // 메뉴를 켤 때
                Time.timeScale = 0f;
            }
            else
            {
                // 메뉴를 끌 때
                Time.timeScale = 1f;
                Cursor.visible = false;
            }
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))   //SettingDataManager스크립트를 씬에 넣어줘야함
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            InventoryUI.Instance.UpdateInventory();
            if(inventoryUI.activeSelf)
            {
                Time.timeScale = 0f;
                isPopUpOpen = true;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                isPopUpOpen = false;
                Cursor.visible = false;
            }

        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("SkillTree")))
        {
            skillTreeUI.SetActive(!skillTreeUI.activeSelf);
            if (inventoryUI.activeSelf)
            {
                Time.timeScale = 0f;
                isPopUpOpen = true;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1f;
                isPopUpOpen = false;
                Cursor.visible = false;
            }
        }
    }
    public void OnClickResume()
    {
        pauseMenuPopUp.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnClickSetting()
    {
        settingPopUp.SetActive(true);
    }
    public void OnClickMainMenu()
    {
        pauseMenuPopUp.SetActive(false);
        GoToMainConfirmationPopupUI.SetActive(true);
    }
    public void OnClickQuit()
    {
        //if (pauseMenuPopUp.activeSelf) return;
        pauseMenuPopUp.SetActive(false);
        exitConfirmationPopupUI.SetActive(true);
        //SetCurrentButton(exitConfirmationPopupUI.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

    }

    // 진행중인 게임을 종료하고 메인 메뉴로 나가시겠습니까?
    public void OnConfirmGoToMainMenu() // 예
    {
        
        SceneManager.LoadScene("MainMenu");
    }
    public void OnCancelGoToMainMenu() // 아니오
    {
        pauseMenuPopUp.SetActive(true);
        GoToMainConfirmationPopupUI.SetActive(false);
    }


    // 종료하시겠습니까?
    public void OnConfirmQuit() // 예
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  //Unity 에디터에서 실행 중일 때만 아래 코드를 실행
        UnityEditor.EditorApplication.isPlaying = false;       //에디터에서 Play 중지
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }
    public void OnCancelQuit() // 아니오
    {
        // 팝업 닫기
        exitConfirmationPopupUI.SetActive(false);
        pauseMenuPopUp.SetActive(true);
        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (exitConfirmationPopupUI.activeSelf) return;
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
    }

}
