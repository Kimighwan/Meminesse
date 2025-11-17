using UnityEngine;

public class VoidbornGoddess_MoveState : MoveState
{
    VoidbornGoddess enemy;
    bool isMoveTimeOver;
    float moveTime;

    public VoidbornGoddess_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        isMoveTimeOver = false;
        SetRandomMoveTIme();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (Time.time > startTIme + moveTime)
            isMoveTimeOver = true;

        if(isMoveTimeOver)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void SetRandomMoveTIme() => moveTime = Random.Range(1, 3);
}
