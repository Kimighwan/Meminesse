using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 6f;      // 돌진 속도
    public float chargeTime = 2f;       // 돌진 하는 시간
}
