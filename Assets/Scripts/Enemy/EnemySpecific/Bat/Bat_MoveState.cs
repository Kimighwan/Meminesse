using System.Security.Principal;
using UnityEngine;

public class Bat_MoveState : MoveState
{
    private Bat enemy;
    private Vector2 moveDir;
    private float distance;

    public Bat_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        // chargeAttack collTime is done : transition state to chargeState
        // yet coolTIme : move around the player

        entity.CheckXPositionForFlip();

        if(entity.CanDetectPlayer())
        {
            moveDir = entity.GetDirectionToPlayer();
            entity.SetVelocity(stateData.moveSpeed, moveDir, 1);
        }
        else
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void Move()
    {
        float y = Mathf.Sin(Time.time * Mathf.Deg2Rad) * stateData.moveSpeed;
        float x = Mathf.Cos(Time.time * Mathf.Deg2Rad) * stateData.moveSpeed;

        entity.SetVelocity(1f, new Vector2(x, y), 1);

        // Move
        //dir = entity.GetDirectionToPlayer();
        //distance = dir.x * dir.x + dir.y * dir.y;
    }
}
