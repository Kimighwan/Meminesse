using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // 씬 전환 시 삭제 여부
    protected bool isDestoryOnLoad = false;     // 디폴트 : 유지

    protected T instance;

    public static T Instance
    {
        get { return Instance; }
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

            if(!isDestoryOnLoad) DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
}
