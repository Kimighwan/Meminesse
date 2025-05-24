using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    public GameObject _popup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _popup.SetActive(false); // 팝업은 처음에 꺼져있어야함
        DontDestroyOnLoad(this); // 씬이 넘어가도 이 스크립트는 파괴되면 안됨
    }
}
