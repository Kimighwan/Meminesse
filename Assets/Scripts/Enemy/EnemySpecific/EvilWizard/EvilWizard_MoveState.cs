using UnityEngine;

public class EvilWizard_MoveState : MoveState
{
    private EvilWizard enemy;

    public EvilWizard_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, EvilWizard enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (isDetectWall || isDetectLedge)
        {
            // 벽과 낭떨어지 감지하면 Idle 상태로 전환
            // 벽과 낭떨어지 이므로 방향 전환 해주기
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
        else if (isPlayerInMeleeAttackRange && enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time)
        {
            int randomValue = RandomAttack();

            if(randomValue == 0)
                stateMachine.ChangeState(enemy.meleeAttack1State);
            else
                stateMachine.ChangeState(enemy.meleeAttack2State);
        }
        else if (isDetectedPlayer)
            stateMachine.ChangeState(enemy.detectState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private int RandomAttack()
    {
        return Random.Range(0, 2);
    }
}
