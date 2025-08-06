using UnityEngine;

public class AssassinCultist_IdleState : IdleState
{
    private AssassinCultist enemy;

    private bool isStart;
    private float currentTime;
    public AssassinCultist_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        isStart = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(enemy.isAttackFinish)
        {
            if(!isStart)
            {
                currentTime = Time.time;
                isStart = true;
            }
            if (isStart && (currentTime + 1 <= Time.time))
                stateMachine.ChangeState(enemy.hidingState);
        }
        else if (isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        else if (isPlayerInMeleeAttackRange && (enemy.firstAttack || enemy.LastAttackTime + enemy.AttackCoolTime <= Time.time))
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (isDetectedPlayer)
        {
            stateMachine.ChangeState(enemy.detectState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
