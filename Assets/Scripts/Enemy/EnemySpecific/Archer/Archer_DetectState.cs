using UnityEngine;

public class Archer_DetectState : DetectState
{
    private Archer enemy;

    public Archer_DetectState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DetectState stateData, Archer enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        // 플레이어가 가까이 있을 때
        if (isPlayerInMeleeAttackRange)
        {
            if(Time.time >= enemy.dodgeState.startTIme + enemy.dodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(enemy.dodgeState);
            }
            else if(isPlayerInRangeAttackRange && enemy.LastAttackTime + enemy.entityData.AttackCoolTime <= Time.time)
            {
                stateMachine.ChangeState(enemy.rangeAttackState);
            }
        }
        else if (!isDetectedPlayer)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
