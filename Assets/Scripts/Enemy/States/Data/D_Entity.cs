using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHp = 30f;                       // 최대 Hp

    public float knockbackSpeed = 10f;              // 넉백 속도
    public Vector2 knockbackAngle;                  // 넉백 각도

    public float playerInMeleeAttackRange = 3f;     // 플레이어 감지 최소 거리
    public float playerInChargeRange = 7f;          // 플레이어 감지 최대 거리

    public float playerDetectRange = 7f;            // 플레이어 추적 최대 거리

    public float wallCheckDistance = 0.2f;          // 벽 체크 거리
    public float ledgeCheckDistance = 0.4f;         // 낭떨어지 체크 거리
    public float groundCheckRadius = 0.3f;          // 넉백후 땅 확인용 반지름

    public float stunRecoveryTIme = 2f;             // 스턴 후 다시 스턴 걸리는 쿨타임

    public LayerMask whatIsPlatform;                // Platform 레이어 마스크
    public LayerMask whatIsPlayer;                  // Player 레이어 마스크
}
