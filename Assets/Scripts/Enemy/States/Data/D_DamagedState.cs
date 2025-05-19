using UnityEngine;

[CreateAssetMenu(fileName = "newDamagedStateData", menuName = "Data/State Data/Damaged State")]
public class D_DamagedState : ScriptableObject
{
    public float knocbackTime = 0.2f;   // 넉백 지속 시간
    public float knocbackSpeed = 20f;   // 넉백 속도
    public Vector2 knocbackAngle;       // 넉백 각도
}
