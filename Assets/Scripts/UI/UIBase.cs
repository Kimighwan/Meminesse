using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    public GameObject invisibleDummyButton;
    public static bool isKeyboardMode = false;       //키보드만 사용하는 모드, 커서 없어짐, 전역변수로 선언 , 플래그 변수임, 처음에는 마우스모드

    public GameObject currentButton;


    protected virtual void Start()
    {
        isKeyboardMode = false;
        Cursor.visible = true;   // Default -> mouse cursor visible
        Cursor.lockState = CursorLockMode.None;

        // 포커스
        SetCurrentButton(invisibleDummyButton);
    }


    protected virtual void Update()
    {
        // 마우스 움직임 감지 → 마우스 모드로 전환
        // 조건 : 지금 키보드모드일 때 마우스 움직임 감지되면
        if ((isKeyboardMode == true) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))    
        {
            Debug.Log("마우스 모드 진입");
            EventSystem.current.SetSelectedGameObject(null);  // 선택 끊기
            isKeyboardMode = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // 키보드 누름 감지 → 키보드 모드로 전환
        // 조건 : 지금 마우스모드일 때 키보드 방향키 눌리면 
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) && (isKeyboardMode == false))  
        {
            Debug.Log("키보드 모드 진입");

            EventSystem.current.SetSelectedGameObject(currentButton); //다시 선택
         
            isKeyboardMode = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;  // Locked은 마우스 잠금용이라 None으로 유지하는 게 UI엔 더 낫다
        }



    }

    public virtual void SetCurrentButton(GameObject gb)
    {
        currentButton = gb;
    }
}
