using UnityEngine;

public interface IPopupUI
{
    void Show();
    void Hide();
    bool IsClosableByEscape { get; }   //ESC 키로 닫을 수 있는 팝업인지 여부
    bool IsActive { get; }
    
    bool HandleEscape();
    /// <summary>
    /// ESC 입력이 들어왔을 때 팝업에서 자체적으로 처리할지 여부를 결정함.
    /// - true  → 팝업이 ESC 입력을 "소비"하여 닫히지 않음 (ex. 서브패널만 닫을 때)
    /// - false → UIManager가 팝업을 닫아도 됨 (일반적인 경우)
    /// </summary>
}

