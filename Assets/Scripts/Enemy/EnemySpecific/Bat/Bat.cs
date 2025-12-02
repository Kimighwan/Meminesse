using UnityEngine;

public class Bat : Entity
{
    public Bat_SleepState sleepState { get; private set; }
    public Bat_IdleState idleState { get; private set; }
    public Bat_ChargeState chargeState { get; private set; }
    public Bat_MoveState moveState { get; private set; }
    public Bat_DetectState detectState { get; private set; }
    public Bat_DamagedState damagedState { get; private set; }
    public Bat_StunState stunState { get; private set; }
    public Bat_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_DetectState detectStateData;
    [SerializeField]
    private D_DamagedState damagedStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    protected float chargeCoolTime;
    protected bool canChargeAttack;

    public override void Start()
    {
        base.Start();

        facingDirection = -1;

        sleepState = new Bat_SleepState(this, stateMachine, "sleep", this);
        idleState = new Bat_IdleState(this, stateMachine, "idle", idleStateData, this);
        chargeState = new Bat_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        moveState = new Bat_MoveState(this, stateMachine, "move", moveStateData, this);
        detectState = new Bat_DetectState(this, stateMachine, "move", detectStateData, this);
        damagedState = new Bat_DamagedState(this, stateMachine, "damaged", damagedStateData, this);
        stunState = new Bat_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Bat_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Init(sleepState);
    }

    public override void Update()
    {
        base.Update();
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

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        
        Gizmos.DrawWireSphere(transform.position, entityData.playerInChargeRadius);
    }
#endif
}
