using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float playerInMeleeAttackRange = 3f;      // 플레이어 감지 최소 거리
    public float playerInChargeRange = 7f;      // 플레이어 감지 최대 거리

    public float playerDetectRange = 7f;

    public float wallCheckDistance = 0.2f;          // 벽 체크 거리
    public float ledgeCheckDistance = 0.4f;         // 낭떨어지 체크 거리

    public LayerMask whatIsPlatform;                // Platform 레이어 마스크
    public LayerMask whatIsPlayer;                  // Player 레이어 마스크
}
