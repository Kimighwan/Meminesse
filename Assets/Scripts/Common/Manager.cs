using UnityEngine;
using UnityEngine.Audio;

public class Manager : MonoBehaviour
{
    private static Manager _instance;

    SceneLoader _scene;
    public static Manager Instance {  get { return _instance; } }
    public static SceneLoader Scene { get { return _instance._scene; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            Init();
        }
        else
            Destroy(gameObject);
    }

    private void Init()
    {
        _scene = new();
    }
}
