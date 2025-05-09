using UnityEngine;

public class FiniteStateMachine
{
    public State currentState {  get; private set; }    // 현재 상태 

    public void Init(State startState)  // 초기 시작 상태 설정
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
