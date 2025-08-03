using UnityEngine;

[CreateAssetMenu(fileName = "newRangeAttackStateData", menuName = "Data/State Data/Range Attack State")]
public class D_RangeAttackState : ScriptableObject
{
    public GameObject projectileGameObject;
    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectiletravelDistance;    // 발사되는 거리
}
