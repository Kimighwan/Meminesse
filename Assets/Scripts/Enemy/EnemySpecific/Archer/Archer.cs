using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Archer : Entity
{
    public Archer_IdleState idleState { get; private set; }
    public Archer_MoveState moveState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;

    public override void Start()
    {
        base.Start();

        idleState = new Archer_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Archer_MoveState(this, stateMachine, "move", moveStateData, this);

        stateMachine.Init(idleState);
    }

    public override void Damaged(float damage, Vector2 position, bool isStun = false)
    {
        base.Damaged(damage, position, isStun);
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
