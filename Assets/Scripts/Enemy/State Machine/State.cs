using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTIme;  // ���� ���� �ð�

    protected string animBoolName;  // �ִϸ��̼� �̸�

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // ���� ����
    {
        Debug.Log($"{stateMachine.currentState} ���� ����");
        startTIme = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoCheck();
    }

    public virtual void Exit()  // ���� ����
    {
        Debug.Log($"{stateMachine.currentState} ���� ����");
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicalUpdate() // Update���� ���
    {

    }

    public virtual void PhysicsUpdate()  // FixedUpdate���� ���
    {
        DoCheck();
    }

    public virtual void DoCheck()       // ���� üũ �Լ����� ��� �Լ�
    {

    }
}
