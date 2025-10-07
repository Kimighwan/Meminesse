using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 인게임 씬 관리 
public class IngameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPopUp;
    [SerializeField] private GameObject settingPopUp;

    [SerializeField] private GameObject GoToMainConfirmationPopupUI; // 진행중인 게임을 종료하고 메인 메뉴로 나가시겠습니까?
    [SerializeField] private GameObject exitConfirmationPopupUI;   // 종료하시겠습니까? 

    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject inventoryUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 리팩토링 시급 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = pauseMenuPopUp.activeSelf;
            if (settingPopUp.activeSelf)
            {
                settingPopUp.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (GoToMainConfirmationPopupUI.activeSelf)
            {
                GoToMainConfirmationPopupUI.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (exitConfirmationPopupUI.activeSelf)
            {
                exitConfirmationPopupUI.SetActive(false);
                pauseMenuPopUp.SetActive(true);
            }
            else if (skillTreeUI.activeSelf)
            {
                skillTreeUI.SetActive(false);
            }
            else if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
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
            }
        }

        if (Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            Time.timeScale = inventoryUI.activeSelf ? 0f : 1f;
        }

        if(Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("SkillTree")))
        {
            skillTreeUI.SetActive(!skillTreeUI.activeSelf);
            Time.timeScale = skillTreeUI.activeSelf ? 0f : 1f;
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

}