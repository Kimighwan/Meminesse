using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy1 : Entity
{
    // 여기에 Enemy1만의 State 선언
    public E1_IdleState idleState {  get; private set; }
    public E1_MoveState moveState { get; private set; }

    // 여기에 Enemy1만의 StateData 선언
    [SerializeField]
    private D_IdelState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        // 여기에서 선언한 State들 새로 생성하여 할당
        idleState = new E1_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new E1_MoveState(this, stateMachine, "move", moveStateData, this);

        // 초기 State 초기화 해주기
        stateMachine.Init(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
