using UnityEngine;

public class Archer_MoveState : MoveState
{
    private Archer enemy;

    public Archer_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // TODO : transition to playerDetectedState and attackState

        if(isDetectWall || !isDetectLedge)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }      

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
