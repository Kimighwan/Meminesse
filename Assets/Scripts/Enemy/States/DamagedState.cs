using UnityEditor.Overlays;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DamagedState : State
{
    protected D_DamagedState stateData;
    protected bool isAnimationDone;

    public DamagedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DamagedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        entity.animationToStatemachine.damageState = this;
        entity.SetVelocity(0f);
        isAnimationDone = false;
        entity.Knockback(stateData.knocbackSpeed, stateData.knocbackAngle, entity.LastDamagedDirection);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void DamagedAnimationDone()
    {
        isAnimationDone = true;
    }
}
