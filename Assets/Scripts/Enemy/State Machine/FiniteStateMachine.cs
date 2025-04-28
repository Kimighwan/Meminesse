using UnityEngine;

public class FiniteStateMachine
{
    public State currentState {  get; private set; }    // ���� ����

    public void Init(State startState)  // �ʱ� ���� ���� ����
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
