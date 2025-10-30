using UnityEngine;
using System.Collections.Generic;

// 아직 안씀
public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> popups = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IPopupUI topPopup = FindTopmostActivePopup();
            if (topPopup != null && topPopup.IsClosableByEscape)
            {
                topPopup.Hide();
            }
        }
    }

    private IPopupUI FindTopmostActivePopup()
    {
        for (int i = popups.Count - 1; i >= 0; i--)
        {
            var popup = popups[i].GetComponent<IPopupUI>();
            if (popup != null && popup.IsActive)
                return popup;
        }
        return null;
    }
}
