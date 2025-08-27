using UnityEngine;
using System.Collections;

public class VoidbornGoddess_Cast1State : IdleState
{
    private VoidbornGoddess enemy;
    private int count;
    private float timer;
    private float delayTime = 0.5f;
    private bool stop;
    public VoidbornGoddess_Cast1State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, VoidbornGoddess enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        count = 0;
        stop = false;
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

        if(count < 15)
        {
            timer += Time.deltaTime;

            if (!stop)
            {
                stop = true;

                Debug.Log("탄막 생성");
                GameObject bullet = GameObject.Instantiate(Resources.Load("Enemy/BulltetA"), enemy.rangeAttackPosition) as GameObject;
                Rigidbody2D bulletRigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dirVec = enemy.GetDirectionToPlayer();
                Vector2 ranVec = new Vector2(Random.Range(0f, 2f), Random.Range(-3f, 3f));
                //Vector2 ranVec = new Vector2(1, Mathf.Cos(Mathf.PI * 10 * Time.time));
                dirVec += ranVec;
                bulletRigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);
                count++;
            }

            if (timer >= delayTime)
            {
                timer = 0f;
                stop = false;
            }
        }
        else
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
