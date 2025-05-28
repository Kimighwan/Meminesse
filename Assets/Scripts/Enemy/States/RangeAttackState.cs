using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class RangeAttackState : AttackState
{
    protected D_RangeAttackState stateData;

    protected GameObject projectileGameObject;
    protected Projectile projectileScript;
    protected Transform tartgetPos;

    public RangeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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

        tartgetPos = entity.PlayerTransformForRangeAttack();
        AttackDirectionFlip();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        AttackDirectionFlip();
        projectileGameObject = GameObject.Instantiate(stateData.projectileGameObject, attackPosition.position, attackPosition.rotation);
        projectileScript = projectileGameObject.GetComponent<Projectile>();
        projectileScript.InitProjectile(stateData.projectileSpeed, stateData.projectiletravelDistance, stateData.projectileDamage, tartgetPos.position);
    }

    private void AttackDirectionFlip()
    {
        if(tartgetPos.position.x > entity.transform.position.x && entity.facingDirection == -1)
        {
            entity.Flip();
        }
        else if(tartgetPos.position.x < entity.transform.position.x && entity.facingDirection == 1)
        {
            entity.Flip();
        }
    }
}
