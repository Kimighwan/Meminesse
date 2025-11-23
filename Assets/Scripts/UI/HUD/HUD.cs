using UnityEngine;

public class HUD : SingletonBehaviour<HUD>
{
    [SerializeField] private HpUI hpBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpBar.UpdateHearts(); 
    }
    public void UpdateHUD() => hpBar.UpdateHearts();
}
