using UnityEngine;

public class AssassinCultist_CommingState : IdleState
{
    private AssassinCultist enemy;
    public AssassinCultist_CommingState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
