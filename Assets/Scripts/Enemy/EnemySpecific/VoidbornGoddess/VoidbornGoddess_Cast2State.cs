using UnityEngine;

public class VoidbornGoddess_Cast2State : IdleState
{
    private VoidbornGoddess enemy;
    private float time;
    private bool check;
    public VoidbornGoddess_Cast2State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        check = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishTransition()
    {
        base.FinishTransition();
    }

    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if(isIdleTimeOver)
            stateMachine.ChangeState(enemy.bigDespawnState);
        else
        {
            if(!check)
            {
                check = true;
                int randomPosition = Random.Range(0, 5);
                GameObject.Instantiate(Resources.Load("Prefabs/Projectile/VoidbornGoddess_Spell1"), enemy.spellPositions[randomPosition].transform);
                Debug.Log($"손 소환 / 위치 : {randomPosition}");
                time = Time.time;
            }

            if (time + 1 <= Time.time) check = false;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
