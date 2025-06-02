using UnityEngine;

public class Bat : Entity
{
    public Bat_SleepState sleepState { get; private set; }
    public Bat_IdleState idleState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;

    public override void Start()
    {
        base.Start();

        facingDirection = -1;

        sleepState = new Bat_SleepState(this, stateMachine, "sleep", this);
        idleState = new Bat_IdleState(this, stateMachine, "idle", idleStateData, this);

        stateMachine.Init(sleepState);
    }
}
