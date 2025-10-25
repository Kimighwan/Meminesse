using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{

    [Header("기본 포커싱용 더미 버튼")]
    public GameObject invisibleDummyButton;

    public static bool isKeyboardMode = false; 

    public GameObject currentButton;


    protected virtual void Start()
    {
        isKeyboardMode = false;
        Cursor.visible = true;   // Default -> mouse cursor visible
        Cursor.lockState = CursorLockMode.None;

        SetCurrentButton(invisibleDummyButton);
    }


    protected virtual void Update()
    {
        // mouse mode
        if ((isKeyboardMode == true) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
        {
            Debug.Log("마우스 모드 진입");
            EventSystem.current.SetSelectedGameObject(null);  // 선택 끊기
            isKeyboardMode = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // keyboard mode
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) && (isKeyboardMode == false))
        {
            Debug.Log("키보드 모드 진입");

            EventSystem.current.SetSelectedGameObject(currentButton); //다시 선택
            SetCurrentButton(currentButton);

            isKeyboardMode = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
        }
    }

    
    public virtual void SetCurrentButton(GameObject gb)
    {
        currentButton = gb;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gb);   
    }
}
