using UnityEngine;

public class Bat_ChargeState : ChargeState
{
    private Bat enemy;
    private Vector2 moveDir;

    public Bat_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isDetectedPlayer = entity.CheckPlayerInDetectRangeTpyeCircle();
    }

    public override void Enter()
    {
        base.Enter();

        moveDir = entity.GetDirectionToPlayer();
        entity.SetVelocity(stateData.chargeSpeed, moveDir, 1);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isChargeAnimationFinished)
        {
            //  : transition state
            // 1) in chargeRange : to moveState
            // 2) out chargeRange : to detectState
            //if(isPlayerInChargeRange)
            //    stateMachine.ChangeState(enemy.moveState);
            //else
            //    stateMachine.ChangeState(enemy.detectState);
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    
}
