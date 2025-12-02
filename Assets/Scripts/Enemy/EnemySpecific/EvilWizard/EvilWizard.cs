using UnityEngine;

public class EvilWizard : Entity
{
    public EvilWizard_IdleState idleState { get; private set; }
    public EvilWizard_MoveState moveState { get; private set; }
    public EvilWizard_MeleeAttack1State meleeAttack1State { get; private set; }
    public EvilWizard_MeleeAttack2State meleeAttack2State { get; private set; }
    public EvilWizard_DetechState detectState { get; private set; }
    public EvilWizard_DamagedState damagedState { get; private set; }
    public EvilWizard_DeadState deadState { get; private set; }

    [SerializeField] private D_IdleState idleStateData;
    [SerializeField] private D_MoveState moveStateData;
    [SerializeField] private D_MeleeAttackState meleeAttack1StateData;
    [SerializeField] private D_MeleeAttackState meleeAttack2StateData;
    [SerializeField] private D_DetectState detectStateData;
    [SerializeField] private D_DamagedState damagedStateData;
    [SerializeField] private D_DeadState deadStateData;

    [SerializeField] private Transform meleeAttack1Position;
    [SerializeField] private Transform meleeAttack2Position;

    public float LastAttackTime { get; set; }


    public override void Start()
    {
        base.Start();

        idleState = new EvilWizard_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new EvilWizard_MoveState(this, stateMachine, "move", moveStateData, this);
        meleeAttack1State = new EvilWizard_MeleeAttack1State(this, stateMachine, "attack1", meleeAttack1Position, meleeAttack1StateData, this);
        meleeAttack2State = new EvilWizard_MeleeAttack2State(this, stateMachine, "attack2", meleeAttack2Position, meleeAttack2StateData, this);
        detectState = new EvilWizard_DetechState(this, stateMachine, "detech", detectStateData, this);
        damagedState = new EvilWizard_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        deadState = new EvilWizard_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun, float defIgnore = 0f)
    {
        base.Damaged(damage, position, isStun);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else
        {
            stateMachine.ChangeState(damagedState);
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 근접 공격 범위 표시
        Gizmos.DrawWireSphere(meleeAttack1Position.position, meleeAttack1StateData.attackRadius);
        Gizmos.DrawWireSphere(meleeAttack2Position.position, meleeAttack2StateData.attackRadius);
    }
#endif
}
