using UnityEngine;

public class NomalCultist_DeadState : DeadState
{
    private NomalCultist enemy;
    public NomalCultist_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, NomalCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
