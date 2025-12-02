using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LobbyScene,
    GameScene,
    Grid_Su,
}

public class SceneLoader : SingletonBehaviour<SceneLoader>
{ 
    // 씬 불러오기
    public void LoadScene(SceneType sceneType)  
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    // 씬 다시 불러오기
    public void ReloadScene()               
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 비동기 방식으로 씬 불러오기
    public AsyncOperation LoadSceneAsync(SceneType sceneType)
    {
        Time.timeScale = 1f;
        return SceneManager.LoadSceneAsync(sceneType.ToString());
    }
}
