using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    public GameObject firstButton;
    public static bool isKeyboardMode = false;       //키보드만 사용하는 모드, 커서 없어짐, 전역변수로 선언 , 플래그 변수임, 처음에는 마우스모드

    private GameObject currentButton;


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

        // 처음에는 마우스모드로 시작하는데 그 조건으로 인하여 이 조건문으로 들어와서 맨 처음 버튼이 미리 선택되어있는 오류가 발생함    -> 지금은 아님 ???
        //  키보드 누름 감지 → 키보드 모드로 전환
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) && (isKeyboardMode == false))   //조건 : 지금 마우스모드일 때 키보드 방향키 눌리면 
        {
            Debug.Log("키보드 모드 진입");
            EventSystem.current.SetSelectedGameObject(currentButton); //다시 선택
            isKeyboardMode = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;  // Locked은 마우스 잠금용이라 None으로 유지하는 게 UI엔 더 낫다
        }

    }

    public void SetCurrentButton(GameObject gb)
    {
        currentButton = gb;
        Debug.Log($"현재 마우스로 가리킨 오브젝트 : {gb.name}");
    }
}
