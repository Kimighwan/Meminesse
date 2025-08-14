using UnityEditor;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerController.AttackType attackType;
    private float attackDamage;
    private Vector2 playerPosition;
    private bool isStunAttack;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            Entity hit = collider.gameObject.GetComponent<Entity>();
            Debug.Log("Enemy hit, Type: " + attackType + ", Damage: " + attackDamage);
            hit.Damaged(attackDamage, playerPosition, isStunAttack);
        }
    }

    #region getters/setters

    public void setAttackType(PlayerController.AttackType type)
    {
        attackType = type;
    }

    public void setAttackDamage(float damage)
    {
        attackDamage = damage;
    }

    public void setPlayerPosition(Vector2 position)
    {
        playerPosition = position;
    }

    public void setStun(bool stun)
    {
        isStunAttack = stun;
    }

    #endregion
}