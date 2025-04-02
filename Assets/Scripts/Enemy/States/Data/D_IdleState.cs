using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State")]
public class D_IdelState : ScriptableObject
{
    public float minIdleTime = 1f;           // Idle 상태 최소 지속 시간
    public float maxIdleTime = 2f;           // Idle 상태 최대 지속 시간
}
