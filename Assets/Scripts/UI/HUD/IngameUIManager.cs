using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class IngameUIManager : UIBase
{
    [SerializeField] private GameObject pauseMenuPopUp;
    [SerializeField] private GameObject settingPopUp;

    [SerializeField] private GameObject GoToMainConfirmationPopupUI; // 진행중인 게임을 종료하고 메인 메뉴로 나가시겠습니까?
    [SerializeField] private GameObject exitConfirmationPopupUI;   // 종료하시겠습니까? 

    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject inventoryUI;

    [Header("First Buttons")]
    [SerializeField] private GameObject pauseMenuFirstButton;
    [SerializeField] private GameObject skillTreeFirstButton;
    [SerializeField] private GameObject inventoryFirstButton;

    private Dictionary<GameObject, GameObject> firstButtonMap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        Cursor.visible = false;

        Time.timeScale = 1;

        firstButtonMap = new Dictionary<GameObject, GameObject>
        {
            { pauseMenuPopUp, pauseMenuFirstButton },
            { skillTreeUI,    skillTreeFirstButton },
            { inventoryUI,    inventoryFirstButton }
        };

        Inventory.Instance.UpdateMoney();
        Inventory.Instance.UpdateWeaponUI(PlayerDataManager.Instance.GetWeaponStep());
        InventoryUI.Instance.UpdateInventory();
        HpUIManager.Instance.UpdateHearts();
    }

    // Update is called once per frame
    new void Update()
    {
        HandlePopup();
    }
    public void HandlePopup()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPopUp.activeSelf)
            {
                settingPopUp.SetActive(false);
                pauseMenuPopUp.SetActive(true);
                if (firstButtonMap.TryGetValue(pauseMenuPopUp, out var pauseFirstButton))
                    SetCurrentButton(pauseFirstButton);
                return;
            }

            else
            {
                ToggleUI(pauseMenuPopUp);
                SetCurrentButton(pauseMenuPopUp.transform.Find("resume").gameObject);
            }
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))
        {
            ToggleUI(inventoryUI);
            HpUIManager.Instance.UpdateHearts(); 
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("SkillTree")))
        {
            ToggleUI(skillTreeUI);
        }
    }

    private void ToggleUI(GameObject ui)
    {
        bool isActive = ui.activeSelf;
        if(ui != settingPopUp) CloseAll();

        if (!isActive)
        {
            ui.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;

            if (firstButtonMap.TryGetValue(ui, out var firstButton))
            {
                SetCurrentButton(firstButton);
            }
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
            SetCurrentButton(invisibleDummyButton);
        }
    }

    private void CloseAll()
    {
        pauseMenuPopUp.SetActive(false);
        skillTreeUI.SetActive(false);
        inventoryUI.SetActive(false);
        settingPopUp.SetActive(false);
        GoToMainConfirmationPopupUI.SetActive(false);
        exitConfirmationPopupUI.SetActive(false);

        HpUIManager.Instance.UpdateHearts();
    }

    public void OnClickResume()
    {
        ToggleUI(pauseMenuPopUp);
    }
    public void OnClickSetting()
    {
        settingPopUp.SetActive(true);
    }
    public void OnClickMainMenu()
    {
        CloseAll();
        GoToMainConfirmationPopupUI.SetActive(true);
        SetCurrentButton(GoToMainConfirmationPopupUI.transform.Find("Yes").gameObject);
    }
    public void OnClickQuit()
    {
        CloseAll();
        exitConfirmationPopupUI.SetActive(true);
        SetCurrentButton(exitConfirmationPopupUI.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

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
#if UNITY_EDITOR                
        UnityEditor.EditorApplication.isPlaying = false;    
        Application.Quit();       //실제 게임에서 종료
#endif
    }
    public void OnCancelQuit() // 아니오
    {
        exitConfirmationPopupUI.SetActive(false);
        pauseMenuPopUp.SetActive(true);

        if (exitConfirmationPopupUI.activeSelf) return;
    }

    public override void SetCurrentButton(GameObject gb)
    {
        base.SetCurrentButton(gb);
        if (EventSystem.current.currentSelectedGameObject != gb)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(gb);
        }
    }

}
