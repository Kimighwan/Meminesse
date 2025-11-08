using UnityEngine;

public class TimeManager : SingletonBehaviour<TimeManager>
{
    private float defaultTimeScale = 1f;
    private float currentTimeScale = 1f;

    private bool isPaused = false;

    // 게임 일시정지
    public void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        currentTimeScale = 0f;
        Time.timeScale = currentTimeScale;
        Debug.Log("[TimeManager] Game Paused");
    }

    // 게임 재개
    public void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        currentTimeScale = defaultTimeScale;
        Time.timeScale = currentTimeScale;
        Debug.Log("[TimeManager] Game Resumed");
    }

    // 슬로우모션 등 외부 제어
    public void SetTimeScale(float scale)
    {
        currentTimeScale = Mathf.Clamp(scale, 0f, 1f);
        Time.timeScale = currentTimeScale;
    }

    public float GetTimeScale() => currentTimeScale;
    public bool IsPaused() => isPaused;
}
