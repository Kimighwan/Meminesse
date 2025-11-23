using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject SkillTreePopUp;

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

        //스킬트리 팝업이 떠있는 경우 - skillTree는 UIBase로 관리x
        if(SkillTreePopUp.activeSelf)
        {
            SkillTreePopUp.SetActive(false);
            return;
        }

        // 2) 팝업이 아무것도 없다면 PausePopUp을 연다
        UIManager.Instance.OpenPopup<PausePopUp>("PausePopUp");
    }


    private void HandleInventory()
    {
        if (!Input.GetKeyDown(SettingDataManager.Instance.GetKeyCode("Inventory")))
            return;

        if (UIManager.Instance.HasActivePopup)
        {
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
        if (UIManager.Instance.HasActivePopup)
        {
            UIManager.Instance.CloseTopPopup();
            HUD.Instance.UpdateHUD();
        }
        else if (SkillTreePopUp.activeSelf)
        {
            SkillTreePopUp.SetActive(false);
            HUD.Instance.UpdateHUD();
        }
        else
        {
            SkillTreePopUp.SetActive(true);
        }
    }
}
