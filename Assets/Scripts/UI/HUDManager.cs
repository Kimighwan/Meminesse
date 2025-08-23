using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuPopUp;

    [SerializeField]
    private GameObject exitConfirmationPopupUI;   // 키보드 모드 오류 해결용

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuPopUp.SetActive(true);
            // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 게임 멈추기

            if (Input.GetKeyDown(KeyCode.Escape)) OnClickResume();
        }
    }
    public void OnClickResume()
    {
        pauseMenuPopUp.SetActive(false);
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 게임 다시 진행
    }
    public void OnClickSetting()
    {
        // SceneManager.LoadScene("");
    }
    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnClickQuit()
    {
        if (pauseMenuPopUp.activeSelf) return;
        exitConfirmationPopupUI.SetActive(true);
        //SetCurrentButton(exitConfirmationPopupUI.transform.Find("Yes").gameObject); // 팝업의 Yes 버튼에 포커스 설정

    }
}
