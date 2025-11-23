using TMPro;
using UnityEngine;

public class TopSkillUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberTxt;

    int number;

    public void Init(int number)
    {
        this.number = number;

        if (number == 1)
        {
            this.numberTxt.text = "<첫 번째 상위 패시브>";
        }
        else if(number == 2)
        {
            this.numberTxt.text = "<두 번째 상위 패시브>";
        }
        else if(number == 3)
        {
            this.numberTxt.text = "<세 번째 상위 패시브>";
        }
    }

    public void OnClickSelectBtn(int index)
    {
        PlayerDataManager.Instance.SetTopPassive(number, index);
        PlayerDataManager.Instance.Save();
        Destroy(gameObject);
    }
}
