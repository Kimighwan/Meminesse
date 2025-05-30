using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject firstButton;
    public static bool isKeyboardMode = false;       //키보드만 사용하는 모드, 커서 없어짐, 전역변수로 선언 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.visible = true;   // Default -> mouse cursor visible
        Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(firstButton);   // Default -> first button is selected
    }

    // Update is called once per frame 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))    //UpArrow, DownArrow -> cursor invisible
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    public void OnClickNewGame()
    {
        
    }

    public void OnClickLoadGame()
    {
        SceneManager.LoadScene("ReloadScene");   // 세이브 파일 불러오기 씬으로 이동
    }

    public void OnClickSetting()
    {
    }

    public void OnClickQuit()     //Quit button
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  //Unity 에디터에서 실행 중일 때만 아래 코드를 실행
        UnityEditor.EditorApplication.isPlaying = false;       //에디터에서 Play 중지
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }


}
