using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class VoidbornGoddess : Entity
{
    public VoidbornGoddess_IdleState idleState { get; private set; }
    public VoidbornGoddess_SmallSpawnState smallSpawnState { get; private set; }
    public VoidbornGoddess_SmallDespawnState smallDespawnState { get; private set; }
    public VoidbornGoddess_BigSpawnState bigSpawnState { get; private set; }
    public VoidbornGoddess_BigDespawnState bigDespawnState { get; private set; }
    public VoidbornGoddess_Cast2State cast2State { get; private set; }
    public VoidbornGoddess_MoveState moveState { get; private set; }
    public VoidbornGoddess_MeleeAttackState meleeAttackState { get; private set; }
    public VoidbornGoddess_Cast1State cast1State { get; private set; }
    public VoidbornGoddess_DamagedState damagedState { get; private set; }
    public VoidbornGoddess_StunState stunState { get; private set; }
    public VoidbornGoddess_DeadState deadState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_IdleState smallSpawnStateData;
    [SerializeField] private D_IdleState smallDespawnStateData;
    [SerializeField] private D_IdleState bigSpawnStateData;
    [SerializeField] private D_IdleState bigDespawnStateData;
    [SerializeField] private D_IdleState cast2StateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_MeleeAttackState meleeAttackStateData;
    [SerializeField] private D_RangeAttackState cast1StateData;
    [SerializeField] private D_DamagedState dmagedStateData;
    [SerializeField] private D_StunState stunStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Transform meleeAttackPosition;
    [SerializeField] private Transform rangeAttackPosition;

    public List<GameObject> spellPositions;

    public override void Start()
    {
        base.Start();

        facingDirection = -1;
        defaultDirection = -1;

        idleState = new VoidbornGoddess_IdleState(this, stateMachine, "idle", idleStateData, this);
        smallSpawnState = new VoidbornGoddess_SmallSpawnState(this, stateMachine, "smallSpawn", smallSpawnStateData, this);
        smallDespawnState = new VoidbornGoddess_SmallDespawnState(this, stateMachine, "smallDespawn", smallDespawnStateData, this);
        bigSpawnState = new VoidbornGoddess_BigSpawnState(this, stateMachine, "bigSpawn", bigSpawnStateData, this);
        bigDespawnState = new VoidbornGoddess_BigDespawnState(this, stateMachine, "bigDespawn", bigDespawnStateData, this);
        cast2State = new VoidbornGoddess_Cast2State(this, stateMachine, "cast2", cast2StateData, this);
        moveState = new VoidbornGoddess_MoveState(this, stateMachine, "move", moveStateData, this);
        meleeAttackState = new VoidbornGoddess_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        cast1State = new VoidbornGoddess_Cast1State(this, stateMachine, "cast1", rangeAttackPosition, cast1StateData, this);
        damagedState = new VoidbornGoddess_DamagedState(this, stateMachine, "damaged", dmagedStateData, this);
        stunState = new VoidbornGoddess_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new VoidbornGoddess_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(smallDespawnState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun)
    {
        base.Damaged(damage, position, isStun);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStun && IsStun && stateMachine.currentState != stunState)
        {
            // 스턴 공격이 들어오면 // IsStun는 스턴 상태 전환 가능한지 체크

            stateMachine.ChangeState(stunState);
            SetIsStun(false);   // 스턴 불가 상태로 설정
            lastStunTime = Time.time;
        }
        else
        {
            stateMachine.ChangeState(damagedState);
        }
    }
}
