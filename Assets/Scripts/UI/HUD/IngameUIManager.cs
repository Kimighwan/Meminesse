using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField] private UIBase skillTreeUI; // 추후에 삭제

    void Update()
    {
        
        HandlePause();
        HandleInventory();
        HandleSkillTree();
    }

    private void HandlePause()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        // 1) 다른 팝업이 떠 있다면 → UIManager가 자동으로 ESC 처리해야 함
        if (UIManager.Instance.HasActivePopup)
        {
            // 단, PausePopUp이 아닌 팝업이면 닫기만 하고 종료
            UIBase top = UIManager.Instance.GetTopPopup();

            if (!(top is PausePopUp))
            {
                UIManager.Instance.CloseTopPopup();
                return;
            }

            // 최상단이 PausePopUp이라면 Pause를 닫음
            UIManager.Instance.ClosePopup(top);
            return;
        }

        // 2) 팝업이 아무것도 없다면 PausePopUp을 연다
        
        UIManager.Instance.OpenPopup<PausePopUp>("PausePopUp");
        Debug.Log("]]]]]]]]]]]]]]]]]]]]]escape감지");
    }


    private void HandleInventory()
    {
        if (!Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))
            return;

        if (UIManager.Instance.HasActivePopup)
        {
            // 최상단 팝업 닫기
            UIManager.Instance.CloseTopPopup();
            HUD.Instance.UpdateHUD();
        }
        else
        {
            UIManager.Instance.OpenPopup<Inventory>("Inventory");
            HUD.Instance.UpdateHUD();
        }
    }


    private void HandleSkillTree()
    {
        if (!Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("SkillTree")))
            return;

        /*
        if (UIManager.Instance.HasActivePopup)
        {
            UIManager.Instance.CloseTopPopup();
            HUD.Instance.UpdateHUD();
        }
        else
        {
            UIManager.Instance.OpenPopup<SkillTree>("SkillTree");
        }
        */

        // 추후에 밑 코드 삭제 후 위에있는 코드로 사용
        if(skillTreeUI.gameObject.activeSelf)
        {
            skillTreeUI.gameObject.SetActive(false);
            HUD.Instance.UpdateHUD();
            TimeManager.Instance.Resume();
        }
        else
        {
            skillTreeUI.gameObject.SetActive(true);
            TimeManager.Instance.Pause();
        }
    }
}
