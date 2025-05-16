using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject firstButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);   //게임 시작시 디폴트 : 첫번째 버튼이 선택되어있는 상태
    }

    // Update is called once per frame 
    void Update()
    {
        
    }

    public void OnClickNewGame()
    {
    }

    public void OnClickLoadGame()
    {
    }  

    public void OnClickSetting()
    {
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    
}
