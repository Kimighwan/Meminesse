using UnityEngine;

[CreateAssetMenu(fileName = "newStunState", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float stunTime = 3f;             // 스턴 지속 시간

    public float stunKnocbackTime = 0.2f;   // 스턴시 넉백 지속 시간
    public float stunKnocbackSpeed = 20f;   // 스턴시 넉백 속도
    public Vector2 stunKnocbackAngle;       // 스턴시 넉백 각도
}
