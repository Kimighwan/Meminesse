using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour, IPopupUI
{
    [SerializeField] private GameObject rootObject;

    [Header("기본 포커싱용 더미 버튼")]
    [SerializeField] protected GameObject invisibleDummyButton;

    [SerializeField] private bool closableByEscape = true;
    public static bool isKeyboardMode = false; 

    public GameObject currentButton;

    protected virtual void Awake()
    {
        if (rootObject == null)
        {
            rootObject = gameObject;
        }
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
        if (!IsActive)
        {
            return;
        }

        CheckMouseInput();
        CheckKeyboardInput(); 
    }
    protected GameObject RootObject => rootObject != null ? rootObject : gameObject;
    public bool IsClosableByEscape => closableByEscape;
    public bool IsActive => RootObject.activeSelf;

    protected void SetRootObject(GameObject root)
    {
        rootObject = root;
    }
    public virtual void Show()
    {
        if (!IsActive)
        {
            RootObject.SetActive(true);
        }
        UIManager.Instance?.NotifyPopupShown(this);
        OnShown();
        //UIManager.Instance.DebugPopupStack();
    }
    public virtual void Hide()
    {
        if (!IsActive)
        {
            return;
        }

        RootObject.SetActive(false);
        UIManager.Instance?.NotifyPopupHidden(this);
        OnHidden();
        //UIManager.Instance.DebugPopupStack();
    }

    protected virtual void OnShown()
    {
        isKeyboardMode = false;

        if (invisibleDummyButton != null)
        {
            SetCurrentButton(invisibleDummyButton);
        }
    }

    protected virtual void OnHidden()
    {
    }

    public virtual bool HandleEscape()
    {
        return false;  // false - esc버튼으로 팝업 닫기 가능
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
