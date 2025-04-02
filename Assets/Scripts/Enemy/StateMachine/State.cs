using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected float startTIme;      // 상태 시작 시간

    protected string animBoolName;  // 애니메이션 이름
    
    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()     // 상태 진입 시
    {
        Debug.Log($"{stateMachine.currentState} 상태 진입");
        startTIme = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoCheck();
    }

    public virtual void Exit()      // 상태 종료 시
    {
        Debug.Log($"{stateMachine.currentState} 상태 종료");
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicalUpdate() // Update
    {

    }

    public virtual void PhysicsUpdate() // FixedUpdate
    {
        DoCheck();
    }

    public virtual void DoCheck()       // 상태 변수들 체크
    {

    }
}
