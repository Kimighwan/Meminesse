using UnityEngine;

public class Bat_DetectState : DetectState
{
    private Bat enemy;
    private Vector2 moveDir;

    public Bat_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        // if in detectRange, moving to player
        moveDir = entity.GetDirectionToPlayer();
        entity.SetVelocity(stateData.detectSpeed, moveDir, 1);

        entity.CheckXPositionForFlip();

        // 1) out detectRange : to idleState
        // 2) in chargeRange : to chargeState
        if (!isPlayerInRangeAttackRange || !isDetectedPlayer)
            stateMachine.ChangeState(enemy.idleState);
        else if(isPlayerInChargeRange)
        {
            stateMachine.ChangeState(enemy.chargeState);

            //if (Time.time >= enemy.chargeState.eixtChargeStateTime + enemy.chargeState.coolTIme)
                
            //else 
            //    stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
