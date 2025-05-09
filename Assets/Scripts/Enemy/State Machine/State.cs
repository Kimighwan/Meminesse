using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTIme;  // 상태 시작 시간

    protected string animBoolName;  // 애니메이션 이름

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // 상태 진입
    {
        Debug.Log($"{stateMachine.currentState} 상태 진입");
        startTIme = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoCheck();
    }

    public virtual void Exit()  // 상태 종료
    {
        Debug.Log($"{stateMachine.currentState} 상태 종료");
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicalUpdate() // Update에서 사용
    {

    }

    public virtual void PhysicsUpdate()  // FixedUpdate에서 사용
    {
        DoCheck();
    }

    public virtual void DoCheck()       // 상태 체크 함수들을 담는 함수
    {

    }
}
