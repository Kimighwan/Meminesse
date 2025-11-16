using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    [Header("기본 포커싱용 더미 버튼")]
    [SerializeField] protected GameObject invisibleDummyButton;

    [SerializeField] private bool closableByEscape = true;
    public static bool isKeyboardMode = false; 

    public GameObject currentButton;

    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        isKeyboardMode = false;
        if (invisibleDummyButton != null)
        {
            SetCurrentButton(invisibleDummyButton);
        }
    }


    protected virtual void Update()
    {
        CheckMouseInput();
        CheckKeyboardInput(); 
    }
    public bool IsClosableByEscape => closableByEscape;

    public bool IsActive
    {
        get
        {
            return UIManager.Instance.IsPopupOnStack(this);
        }
    }

    public virtual void OnShown()
    {
        isKeyboardMode = false;

        if (invisibleDummyButton != null)
        {
            SetCurrentButton(invisibleDummyButton);
        }
    }

    public virtual void OnHidden()
    {
    }

    public virtual bool HandleEscape()
    {
        return false;
        // false - esc버튼으로 팝업 닫기를 manager에서 처리
        // true - 팝업에서 esc버튼 입력을 자체 처리 
    }

    protected void CheckMouseInput()
    {
        if (!isKeyboardMode)
        {
            return;
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            isKeyboardMode = false;
        }
    }
    protected void CheckKeyboardInput()
    {
        // keyboard mode
        if (isKeyboardMode)
            return;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("UIBase - 키보드 모드 진입");

            EventSystem.current.SetSelectedGameObject(currentButton); //다시 선택
            SetCurrentButton(currentButton);

            isKeyboardMode = true;
        }
    }

    public virtual void SetCurrentButton(GameObject gb)
    {
        currentButton = gb;

        if (EventSystem.current == null)
        {
            return;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gb);   
    }
}
