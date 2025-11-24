using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerController.AttackType attackType;
    private float attackDamage;
    private Vector2 playerPosition;
    private bool isStunAttack;
    private float defIgnore;

    // For top passive skill (expert) bonus hits
    private bool additionalHit = false;
    
    // Cached collider and per-activation hit tracking
    private Collider2D cachedCollider;
    private HashSet<int> alreadyHitObjects = new HashSet<int>();

    private void Awake()
    {
        cachedCollider = GetComponent<Collider2D>();
        if (cachedCollider == null)
        {
            Debug.LogWarning("PlayerAttackHitbox: No Collider2D found on hitbox GameObject.");
        }
    }

    private void OnEnable()
    {
        // Clear hit cache when the hitbox is re-enabled/enabled
        alreadyHitObjects.Clear();

        // Refresh collider for reliable trigger events
        if (cachedCollider != null)
        {
            // Multiple target hits
            StartCoroutine(RefreshCollider());
        }
    }

    private System.Collections.IEnumerator RefreshCollider()
    {
        cachedCollider.enabled = false;
        yield return null;
        cachedCollider.enabled = true;
    }

    public void BeginAttack()
    {
        // Called by PlayerController before activating the hitbox
        // Clears the hit cache
        alreadyHitObjects.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider == null || collider.gameObject == null)
            return;

        // Avoid double-hits during the same hitbox activation
        int id = collider.gameObject.GetInstanceID();
        if (alreadyHitObjects.Contains(id))
            return;

        // Get bonus hits when player has top passive skill(expert) enabled
        // Only enabled when player health is 20 or below
        float bonusAttackMultiplier = 0f;
        int playerHealth = PlayerDataManager.Instance.GetHp();
        if (playerHealth <= 20)
        {
            int topPassiveLevel_expert = PlayerDataManager.Instance.GetTopPassive(1);
            if (topPassiveLevel_expert > 0 && !additionalHit)
            {
                float bonusAttackChance = 0f;

                switch (topPassiveLevel_expert)
                {
                    case 1:
                        bonusAttackMultiplier = 0.3f;
                        bonusAttackChance = 0.1f;
                        break;
                    case 2:
                        bonusAttackMultiplier = 0.6f;
                        bonusAttackChance = 0.15f;
                        break;
                    case 3:
                        bonusAttackMultiplier = 1f;
                        bonusAttackChance = 0.3f;
                        break;
                    default:
                        bonusAttackMultiplier = 0f;
                        bonusAttackChance = 0f;
                        Debug.LogError("Invalid top passive level for expert skill: " + topPassiveLevel_expert);
                        break;
                }

                if (Random.value <= bonusAttackChance)
                {
                    additionalHit = true;
                }
            }
        }
        else
        {
            additionalHit = false;
        }

        Debug.Log("Collider hit: " + collider.gameObject.name);
        if (collider.gameObject.CompareTag("Enemy"))
        {
            alreadyHitObjects.Add(id);
            Entity hit = collider.gameObject.GetComponent<Entity>();
            if (hit != null)
            {
                Debug.Log("Enemy hit, Type: " + attackType + ", Damage: " + attackDamage);
                hit.Damaged(attackDamage, playerPosition, isStunAttack, defIgnore);
                // Bonus hit
                if (additionalHit)
                {
                    float bonusDamage = attackDamage * bonusAttackMultiplier;
                    Debug.Log("Bonus hit triggered, damage: " + bonusDamage);
                    hit.Damaged(bonusDamage, playerPosition, isStunAttack, defIgnore);
                }
            }
        }
        else if (collider.gameObject.CompareTag("Barrier"))
        {
            alreadyHitObjects.Add(id);
            Barrier barrier = collider.gameObject.GetComponent<Barrier>();
            if (barrier != null)
            {
                Debug.Log("Barrier hit, Type: " + attackType + ", Damage: " + attackDamage);
                barrier.Damaged(attackDamage, playerPosition, defIgnore);
                // Bonus hit
                if (additionalHit)
                {
                    float bonusDamage = attackDamage * bonusAttackMultiplier;
                    Debug.Log("Bonus hit triggered, damage: " + bonusDamage);
                    barrier.Damaged(bonusDamage, playerPosition, defIgnore);
                }
            }
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

    public void setDefenceIngore(float ignore)
    {
        defIgnore = ignore;
    }

    #endregion
}