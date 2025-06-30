using UnityEngine;

public class AnimationToStatemachine : MonoBehaviour
{
    public AttackState attackState;
    public DamagedState damageState;
    public ChargeState chargeState;

    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    private void DamagedAnimationDone()
    {
        damageState.DamagedAnimationDone();
        Debug.Log("Bat_Damaged Done!!");
    }

    private void FinishCharge()
    {
        chargeState.FinishCharge();
        Debug.Log("Bat_Charge Done!!");
    }
}
