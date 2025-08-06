using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// 키 조작법 관리
public class KeyController : MonoBehaviour
{
    // Button Number
    // 0:up , 1:down, 2:right, 3:left, 4:attack, 5:jump, 6:dash, 7:skill1, 8:skill2, 9:skill3, 10:map, 11:inventory, 12:skilltree, 13:interect, 14:reset

    [SerializeField]
    private GameObject[] buttons;
    [SerializeField]
    private TextMeshProUGUI[] texts;
    private SettingDataManager settingDataManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnKeyPressed()
    {
        //int num;
        //texts[num].text = "키 입력";

        //사용자가 입력한 키 판별
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                settingDataManager.ChangeKey(selectedKeyCode, key);
                //Debug.Log(texts[num] + "키가 " + key +"에서 " + key + "로 바뀜");
            }
        }
    }

    public void ResetKey()
    {
        settingDataManager.ResetKeyData();
    }
}
