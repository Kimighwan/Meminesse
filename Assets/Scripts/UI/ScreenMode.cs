using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMode : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown;
    //FullScreenMode fullScreenMode;

    // 화면 모드
    public enum ScreenModeOptions
    {
        FullScreenWindow,
        Window
    }

    private void Start()
    {
        List<string> options = new List<string> {
            "전체 화면",
            "창 모드"
        };

        dropdown.ClearOptions();
        dropdown.AddOptions(options);   // 드롭다운에 옵션 추가
        dropdown.onValueChanged.AddListener(index => ChangeFullScreenMode((ScreenModeOptions)index)); //선택된 index를 ScreenModeOptions enum으로 변환→ ChangeFullScreenMode() 실행.

        // 초기 설정
        switch (dropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
                
                break;
            case 1:
                Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
                
                break;
        }
    }


    public void ChangeFullScreenMode(ScreenModeOptions mode)
    {
        switch (mode)
        {
            case ScreenModeOptions.FullScreenWindow:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("전체 화면 모드로 설정됨");
                break;
            case ScreenModeOptions.Window:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("창 모드로 설정됨");
                break;
        }
    }
}