using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State")]
public class D_DodgeState : ScriptableObject
{
    public float dodgeDuration = 2f;
    public float dodgeSpeed = 7f;
    public float dodgeCooldown = 2f;
    public Vector2 dodgeAngle;
}
