using UnityEngine;

public class PausePopUp : UIBase
{
    [SerializeField] private GameObject popupRoot;

    public override void Show()
    {
        SetRootObject(popupRoot);
        Debug.Log($"----팝업 Show 호출");
        base.Show();
    }
}
