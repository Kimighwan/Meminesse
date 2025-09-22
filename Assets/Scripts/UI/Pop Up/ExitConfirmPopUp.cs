using UnityEngine;

public class ExitConfirmPopUp : MonoBehaviour
{

    [SerializeField]
    private GameObject exitConfirmationPopupUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 종료하시겠습니까 - 예
    public void OnConfirmQuit()
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  //Unity 에디터에서 실행 중일 때만 아래 코드를 실행
        UnityEditor.EditorApplication.isPlaying = false;       //에디터에서 Play 중지
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }

    // 종료하시겠습니까 - 아니오
    public void OnCancelQuit()
    {
        // 팝업 닫기
        exitConfirmationPopupUI.SetActive(false);

        // 창이 넘어갔을 때 포커스에 메인에 그대로 있는 문제 해결
        if (exitConfirmationPopupUI.activeSelf) return;
    }
}
