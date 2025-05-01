using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TestEnemy : Entity
{
    public TestEnemy_IdleState idleState { get; private set; }
    public TestEnemy_MoveState moveState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;

    public override void Start()
    {
        base.Start();

        idleState = new TestEnemy_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new TestEnemy_MoveState(this, stateMachine, "move", moveStateData, this);

        stateMachine.Init(idleState);
    }
}
