using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

// 키 조작법 관리
public class KeySetting : MonoBehaviour
{
    // Button Number
    // 0:up , 1:down, 2:right, 3:left, 4:attack, 5:jump, 6:dash, 7:skill1, 8:skill2, 9:skill3, 10:map, 11:inventory, 12:skilltree, 13:interact, 14:heal

    [SerializeField]
    private GameObject[] buttons; // UI 버튼
    [SerializeField]
    private TextMeshProUGUI[] texts; // UI 버튼 텍스트
    [SerializeField]
    private TextMeshProUGUI confirmText; // 위에 뜨는 확인 문구

    private int waitingIndex = -1;    // 현재 리바인딩 중인 키 인덱스. 아무것도 선택되지 않음 : -1
    private bool isRebinding = false; // 지금 입력 대기 중인지


    // 사용자가 입력한 키를 좀 더 보기 좋게 표시하기 위한 딕셔너리
    private Dictionary<KeyCode, string> keyDisplayNames = new Dictionary<KeyCode, string>
    {
        { KeyCode.Mouse0, "마우스 왼쪽" },   // 이미지로 넣으면 좋을듯
        { KeyCode.Mouse1, "마우스 오른쪽" }, // 이미지로 넣으면 좋을듯
        { KeyCode.Mouse2, "마우스 휠" },     // 이미지로 넣으면 좋을듯
        { KeyCode.LeftShift, "Shift" },
        { KeyCode.RightShift, "Shift" },
        { KeyCode.Alpha0, "0" },
        { KeyCode.Alpha1, "1" },
        { KeyCode.Alpha2, "2" },
        { KeyCode.Alpha3, "3" },
        { KeyCode.Alpha4, "4" },
        { KeyCode.Alpha5, "5" },
        { KeyCode.Alpha6, "6" },
        { KeyCode.Alpha7, "7" },
        { KeyCode.Alpha8, "8" },
        { KeyCode.Alpha9, "9" },
        { KeyCode.UpArrow, "▲" },
        { KeyCode.DownArrow, "▼" },
        { KeyCode.LeftArrow, "◄" },
        { KeyCode.RightArrow, "►" },
        { KeyCode.Space, "스페이스" },
        { KeyCode.Return, "Enter" },
        { KeyCode.Escape, "Esc"},
        { KeyCode.BackQuote, "`"}
   
        // 나머지 필요한 키들도 추가
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        // 입력 대기중이고 키가 입력되면
        if (isRebinding && Input.anyKeyDown)
        {
            // 하나씩 순회하면서 사용자가 입력한 키를 확인
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    string keyName = DataManager.Setting.keyDataList[waitingIndex].keyName;   //waitingindex에 값이 들어감(StartRebinding 함수에서 설정됨)

                    // SettingDataManager 통해 키 변경 시도(true일 때만)
                    if (DataManager.Setting.ChangeKey(keyName, key))
                    {
                        Debug.Log($"{keyName} → {key} 로 변경됨");
                        if (keyDisplayNames.ContainsKey(key))  // keyDisplayNames라는 딕셔너리에 key가 들어있는지 확인
                        {
                            ShowConfirmMessage($"{keyName} 키가 {keyDisplayNames[key]}(으)로 변경되었습니다");
                        }
                        else
                        {
                            ShowConfirmMessage($"{keyName} 키가 {key}(으)로 변경되었습니다");
                        }
                     
                    }
                    else
                    {
                        Debug.LogWarning("중복되는 키 설정이 있습니다.");
                        ShowConfirmMessage("중복되는 키 설정이 있습니다.");
                    }

                    isRebinding = false;
                    waitingIndex = -1;
                    RefreshUI(); // UI 업데이트
                    break; // 입력된 키를 찾았으므로 반복문 종료
                }
            }
        }
    }


    // 사용자가 버튼 onClick 시 호출되는 함수
    public void StartRebinding(int index)
    {
        waitingIndex = index;
        isRebinding = true;
        texts[index].text = "키 입력";
    }

    private void RefreshUI()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            KeyCode key = DataManager.Setting.keyDataList[i].keyCode; // KeyCode 순서대로 가져오기

            if (keyDisplayNames.ContainsKey(key))  // keyDisplayNames라는 딕셔너리에 key가 들어있는지 확인
            {
                texts[i].text = keyDisplayNames[key];
            }
            else
            {
                texts[i].text = key.ToString(); // 매핑 안 되어있으면 기본 이름 사용
            }
        }
    }

    //초기화
    public void ResetKey()
    {
        DataManager.Setting.ResetKeyData();
        ShowConfirmMessage("모든 키가 기본값으로 초기화되었습니다");
        RefreshUI();
    }

    // 확인 문구 1초만 표시
    private void ShowConfirmMessage(string msg)
    {
        if (confirmText == null) return;

        confirmText.text = msg;
        confirmText.gameObject.SetActive(true);

        Color c = confirmText.color;
        c.a = 1f;
        confirmText.color = c;

        StopAllCoroutines();
        StartCoroutine(FadeOutConfirmText());
    }

    // 확인 문구 페이드 아웃 효과
    private IEnumerator FadeOutConfirmText()
    {
        yield return new WaitForSeconds(1f); // 1초 동안 기다리자는 말
        float duration = 0.5f; // 페이드 아웃 시간
        float t = 0;

        Color c = confirmText.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / duration);
            confirmText.color = c;
            yield return null;
        }

        confirmText.gameObject.SetActive(false);
    }
}
