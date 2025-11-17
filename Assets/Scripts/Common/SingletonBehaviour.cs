using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // 씬 전환 시 삭제 여부
    protected bool isDestroyOnLoad = false;     // 디폴트 : 유지

    protected static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (instance == null)
        {
            instance = (T)this;

            if(!isDestroyOnLoad) DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
}
