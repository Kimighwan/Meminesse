using UnityEngine;

public class TwistedCultist_TransitionState : IdleState
{
    private TwistedCultist enemy;
    public TwistedCultist_TransitionState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, TwistedCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(doneTransition)
            stateMachine.ChangeState(enemy.meleeAttackState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void FinishTransition()
    {
        base.FinishTransition();
    }
}
