using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject firstButton;
    public static bool isKeyboardMode = false;       //키보드만 사용하는 모드, 커서 없어짐, 전역변수로 선언 , 플래그 변수임, 처음에는 마우스모드

    

    void Start()
    {
        isKeyboardMode = false;
        Cursor.visible = true;   // Default -> mouse cursor visible
        Cursor.lockState = CursorLockMode.None;

        //EventSystem.current.SetSelectedGameObject(firstButton);   // Default -> first button is selected
    }


    void Update()
    {
        
        //  마우스 움직임 감지 → 마우스 모드로 전환
        if ((isKeyboardMode == true) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))    // 조건 : 지금 키보드모드일 때 마우스 움직임 감지되면
        {
            Debug.Log("마우스 모드 진입");
            EventSystem.current.SetSelectedGameObject(null);  // 선택 끊기
            isKeyboardMode = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // 처음에는 마우스모드로 시작하는데 그 조건으로 인하여 이 조건문으로 들어와서 맨 처음 버튼이 미리 선택되어있는 오류가 발생함 
        //  키보드 누름 감지 → 키보드 모드로 전환
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)) && (isKeyboardMode == false))   //조건 : 지금 마우스모드일 때 키보드 방향키 눌리면 
        {
            Debug.Log("키보드 모드 진입");
            EventSystem.current.SetSelectedGameObject(firstButton); //다시 선택
            isKeyboardMode = true;
            Cursor.visible = false;    
            Cursor.lockState = CursorLockMode.Locked;  // Locked은 마우스 잠금용이라 None으로 유지하는 게 UI엔 더 낫다
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
