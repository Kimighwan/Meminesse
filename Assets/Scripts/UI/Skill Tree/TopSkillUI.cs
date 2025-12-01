using System;
using TMPro;
using UnityEngine;

public class TopSkillUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberTxt;

    public Action OnAction;

    int number;

    public void Init(int n)
    {
        number = n;

        if (number == 1)
        {
            numberTxt.text = "<첫 번째 상위 패시브를 선택해주세요.>";
        }
        else if(number == 2)
        {
            numberTxt.text = "<두 번째 상위 패시브를 선택해주세요.>";
        }
        else if(number == 3)
        {
            numberTxt.text = "<세 번째 상위 패시브를 선택해주세요.>";
        }
    }

    public void OnClickSelectBtn(int index)
    {
        PlayerDataManager.Instance.SetTopPassiveLevel(number, index);
        PlayerDataManager.Instance.Save();
        OnAction?.Invoke();
        Destroy(gameObject);
    }
}
