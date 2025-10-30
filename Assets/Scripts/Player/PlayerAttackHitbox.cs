using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private PlayerController.AttackType attackType;
    private float attackDamage;
    private Vector2 playerPosition;
    private bool isStunAttack;
    private float defIgnore;
    
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

        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.CompareTag("Enemy"))
        {
            alreadyHitObjects.Add(id);
            Entity hit = collider.gameObject.GetComponent<Entity>();
            if (hit != null)
            {
                Debug.Log("Enemy hit, Type: " + attackType + ", Damage: " + attackDamage);
                hit.Damaged(attackDamage, playerPosition, isStunAttack, defIgnore);
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