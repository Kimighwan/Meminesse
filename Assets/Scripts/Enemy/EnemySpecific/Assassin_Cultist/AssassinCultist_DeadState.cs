using UnityEngine;

public class AssassinCultist_DeadState : DeadState
{
    private AssassinCultist enemy;
    public AssassinCultist_DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, AssassinCultist enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
