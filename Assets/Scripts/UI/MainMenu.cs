using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : UIBase
{
    

    public void OnClickNewGame()
    {
        
    }

    public void OnClickLoadGame()
    {
        SceneManager.LoadScene("ReloadScene");   // 세이브 파일 불러오기 씬으로 이동
    }

    public void OnClickSetting()
    {
    }

    public void OnClickQuit()     //Quit button
    {
        Debug.Log("Game closed");
#if UNITY_EDITOR                  //Unity 에디터에서 실행 중일 때만 아래 코드를 실행
        UnityEditor.EditorApplication.isPlaying = false;       //에디터에서 Play 중지
#else
        Application.Quit();       //실제 게임에서 종료
#endif
    }


}
