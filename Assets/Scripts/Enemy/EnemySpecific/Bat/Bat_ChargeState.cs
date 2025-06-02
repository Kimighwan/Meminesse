using UnityEngine;

public class Bat_ChargeState : ChargeState
{
    private Bat enemy;

    public Bat_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Bat enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }
}
