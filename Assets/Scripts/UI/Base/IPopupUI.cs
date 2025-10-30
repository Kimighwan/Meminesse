using UnityEngine;

public interface IPopupUI
{
    void Show();
    void Hide();
    bool IsClosableByEscape { get; }
    bool IsActive { get; }
}

