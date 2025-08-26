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
                int randomPosition1 = Random.Range(0, 7);
                int randomPosition2 = Random.Range(0, 7);
                int randomPosition3 = Random.Range(0, 7);

                if(randomPosition1 == randomPosition2)
                {
                    randomPosition2++;
                    randomPosition2 %= 7;
                }
                if((randomPosition2 == randomPosition3) || (randomPosition1 == randomPosition3))
                {
                    randomPosition3++;
                    randomPosition3 %= 7;
                }
                GameObject.Instantiate(Resources.Load("Enemy/VoidbornGoddess_Spell1"), enemy.spellPositions[randomPosition1].transform);
                GameObject.Instantiate(Resources.Load("Enemy/VoidbornGoddess_Spell1"), enemy.spellPositions[randomPosition2].transform);
                GameObject.Instantiate(Resources.Load("Enemy/VoidbornGoddess_Spell1"), enemy.spellPositions[randomPosition3].transform);
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
