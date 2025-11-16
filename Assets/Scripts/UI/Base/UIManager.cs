using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<UIManager>();
            }
            return instance;
        }
    }

    // stack 관리
    private readonly Stack<UIBase> popupStack = new Stack<UIBase>();

    // 떠있는 팝업 있는지
    public bool HasActivePopup => popupStack.Count > 0;

    #region singleton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    // 팝업 생성
    // 예)
    // UIManager.Instance.OpenPopup<ExitPopUp>("ExitPopUp");
    public T OpenPopup<T>(string prefabName) where T : UIBase
    {
        GameObject prefab = Resources.Load<GameObject>($"PopUp/{prefabName}");
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] 팝업 프리팹을 찾을 수 없습니다: {prefabName}");
            return null;
        }

        GameObject instance = Instantiate(prefab);

        T popup = instance.GetComponent<T>();
        if (popup == null)
        {
            Debug.LogError($"[UIManager] {prefabName} 프리팹에 {typeof(T)} 스크립트가 없습니다.");
            return null;
        }

        // push
        popupStack.Push(popup);
        popup.OnShown();

        UpdateGlobalState();


        return popup;
    }

    // 최상단 팝업 닫기
    public void CloseTopPopup()
    {
        if (popupStack.Count == 0)
            return;

        UIBase top = popupStack.Peek();

        // ESC를 소비할지 확인
        if (top.HandleEscape())
            return;

        popupStack.Pop();

        top.OnHidden();
        Destroy(top.gameObject); 

        UpdateGlobalState();
    }

    // 팝업 닫기
    public void ClosePopup(UIBase popup)
    {
        if (popupStack.Count == 0)
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.LogWarning("UIManager - 최상위 팝업만 닫을 수 있음.");
            return;
        }

        // pop
        popupStack.Pop();
        popup.OnHidden();
        Destroy(popup.gameObject);

        UpdateGlobalState();
    }

    // 모든 팝업 닫기
    public void CloseAllPopups()
    {
        while (popupStack.Count > 0)
        {
            UIBase popup = popupStack.Pop();
            popup.OnHidden();
            Destroy(popup.gameObject);
        }
        UpdateGlobalState();
    }
    // 최상위 팝업 가져오기
    public UIBase GetTopPopup()
    {
        if (popupStack.Count == 0)
            return null;
        return popupStack.Peek();
    }

    // 활성화된 팝업인지
    public bool IsPopupOnStack(UIBase popup)
    {
        return popupStack.Contains(popup);
    }

    // 커서/타임스케일 관리
    private void UpdateGlobalState()
    {
        // 커서 제어 보류
        //Cursor.visible = HasActivePopup;
        //Cursor.lockState = HasActivePopup ? CursorLockMode.None : CursorLockMode.Locked;
        if (HasActivePopup) TimeManager.Instance.Pause();
        else TimeManager.Instance.Resume();
    }

    // 디버그
    public void DebugPopupStack()
    {
        Debug.Log("=== [UIManager Popup Stack] ===");
        foreach (var popup in popupStack)
        {
            Debug.Log(popup.name);
        }
    }

}
