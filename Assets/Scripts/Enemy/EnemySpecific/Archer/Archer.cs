using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Archer : Entity
{
    public Archer_IdleState idleState { get; private set; }
    public Archer_MoveState moveState { get; private set; }
    public Archer_DodgeState dodgeState { get; private set; }
    public Archer_DetectState detectState { get; private set; }
    public Archer_DamagedState damagedState { get; private set; }
    public Archer_StunState stunState { get; private set; }
    public Archer_DeadState deadState { get; private set; }
    public Archer_RangeAttackState rangeAttackState { get; private set; }
    

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    public D_DodgeState dodgeStateData;
    [SerializeField]
    private D_DetectState detectStateData;
    [SerializeField]
    private D_DamagedState dmagedStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;
    [SerializeField]
    private D_RangeAttackState rangeAttackStateData;

    [SerializeField]
    private Transform rangeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);
        dodgeState = new Archer_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        damagedState = new Archer_DamagedState(this, stateMachine, "damaged", dmagedStateData, this);
        stunState = new Archer_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Archer_DeadState(this, stateMachine, "dead",  deadStateData, this);
        detectState = new Archer_DetectState(this, stateMachine, "detect", detectStateData, this);
        rangeAttackState = new Archer_RangeAttackState(this, stateMachine, "attack", rangeAttackPosition, rangeAttackStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun = false, float defIgnore = 0f)
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

    public void OnClickDamaged()
    {
        Damaged(10, new Vector2(0, 0), false);
    }

    public void OnClickStun()
    {
        Damaged(5, new Vector2(0, 0), true);
    }

}
