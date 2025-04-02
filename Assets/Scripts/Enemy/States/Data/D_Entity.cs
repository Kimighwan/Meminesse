using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;          // 벽 체크 거리
    public float ledgeCheckDistance = 0.4f;         // 낭떨어지 체크 거리

    public float playerDetectedMinRange = 3f;       // 플레이어 감지 최소 거리
    public float playerDetectedMaxRange = 4f;       // 플래이어 감지 최대 거리

    public float meleeAttackRangeDistance = 1f;     // 근접 공격 감지 범위

    public LayerMask whatIsPlatform;                     // Ground 레이어 마스크
    public LayerMask whatIsPlayer;                  // Player 레이어 마스크
}
