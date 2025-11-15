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
                instance = FindObjectOfType<UIManager>();
            }

            return instance;
        }
    }

    private readonly List<IPopupUI> popupStack = new List<IPopupUI>();

    public bool HasActivePopup => popupStack.Count > 0;

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
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
        {
            return;
        }

        IPopupUI topPopup = GetTopmostPopup();
        if (topPopup == null)
        {
            Debug.Log("UIManager - 열린 팝업이 없습니다.");
            return;
        }

        if (topPopup.HandleEscape())
        {
            return;
        }

        if (topPopup.IsClosableByEscape) topPopup.Hide();
    }

    // 팝업 열기
    public void NotifyPopupShown(IPopupUI popup)
    {
        if (popupStack.Contains(popup))
        {
            popupStack.Remove(popup);
        }
        //DebugPopupStack();
        popupStack.Add(popup);
        UpdateGlobalState();
    }

    // 팝업 닫기
    public void NotifyPopupHidden(IPopupUI popup)
    {
        if (popupStack.Remove(popup))
        {
            UpdateGlobalState();
        }
        
    }

    // 최상위 팝업 가져오기
    private IPopupUI GetTopmostPopup()
    {
        if (popupStack.Count == 0)
        {
            return null;
        }

        return popupStack[popupStack.Count - 1];
    }

    // 커서/타임스케일 관리
    private void UpdateGlobalState()
    {
        bool hasActivePopup = popupStack.Count > 0;
        
        // 커서 제어 보류
        //Cursor.visible = hasActivePopup;
        //Cursor.lockState = hasActivePopup ? CursorLockMode.None : CursorLockMode.Locked;
        //Time.timeScale = hasActivePopup ? 0f : 1f;
        if (hasActivePopup) TimeManager.Instance.Pause();
        else TimeManager.Instance.Resume();
    }

    // 디버그
    public void DebugPopupStack()
    {
        Debug.Log("=== [UIManager Popup Stack] ===");

        if (popupStack.Count == 0)
        {
            Debug.Log("현재 열린 팝업이 없습니다.");
            return;
        }

        for (int i = 0; i < popupStack.Count; i++)
        {
            string popupName = (popupStack[i] as MonoBehaviour)?.gameObject.name ?? "(알 수 없음)";
            Debug.Log($"최상위 팝업은 {GetTopmostPopup()}");
            if (i == popupStack.Count - 1)
                Debug.Log($"[{i}] {popupName}  ← ★ 최상위 팝업");
            else
                Debug.Log($"[{i}] {popupName}");
        }

        Debug.Log("===============================");
    }

}
