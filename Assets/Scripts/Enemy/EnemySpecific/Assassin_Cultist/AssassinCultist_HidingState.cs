using Unity.VisualScripting;
using UnityEngine;

public class AssassinCultist_HidingState : IdleState
{
    private AssassinCultist enemy;

    private bool isStart;
    private float currentTime;
    private float randomTime;
    public AssassinCultist_HidingState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        enemy.rigid.simulated = false;
        isStart = false;
        enemy.isAttackFinish = false;
        RandomTime();
    }

    public override void Exit()
    {
        base.Exit();

        Transform playerTF = enemy.PlayerTransformForRangeAttack();
        if(enemy.facingDirection == 1)
            enemy.transform.position = playerTF.position + new Vector3(-1f, 3f);
        else
            enemy.transform.position = playerTF.position + new Vector3(1f, 3f);

    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();


        if (!isStart)
        {
            currentTime = Time.time;
            isStart = true;
        }
        if (isStart && (currentTime + randomTime <= Time.time))
            stateMachine.ChangeState(enemy.hidingAttackState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void RandomTime()
    {
        randomTime = Random.Range(2f, 6f);
    }
}
