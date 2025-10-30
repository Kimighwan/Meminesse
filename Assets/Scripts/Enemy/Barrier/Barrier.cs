using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] bool higherDefence;
    [SerializeField] bool damagableFromLeft;
    [SerializeField] bool damagableFromRight;
    [SerializeField] float hp = 100;
    private float leftX;
    private float rightX;
    private float defence;

    // For testing purposes
    [SerializeField] bool noDefence;

    void Start()
    {
        if (higherDefence)
            defence = 200;
        else
            defence = 100;

        // For testing purposes
        if (noDefence)
            defence = 0;

        // For directional damage checks
        SetBoundaries();
    }
    
    private void SetBoundaries()
    {
        // Default to object's position in case no collider is found
        leftX = transform.position.x;
        rightX = transform.position.x;

        // Use collider bounds to set leftmost and rightmost X positions
        Collider2D col2D = GetComponent<Collider2D>();

        if (col2D != null)
        {
            leftX = col2D.bounds.min.x;
            rightX = col2D.bounds.max.x;
        }
    }

    public void Damaged(float damage, Vector2 position, float defIgnore)
    {
        if (position.x < leftX && !damagableFromLeft) return;
        if (position.x > rightX && !damagableFromRight) return;

        // Damage calculation
        float realDefence = defence * (1 - defIgnore);
        float damageReduction = 1 - (realDefence / 100);
        damage *= damageReduction;

        // Min damage
        if (damage <= 0)
            damage = 1;

        hp -= damage;

        // Object permanently destroyed(not temporarily disabled)
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}

/*

Base Attack Damage: 100

Real DEF = DEF * (1 - Penetration)
Damage Reduction% = 1 - Real DEF / 100
Real Damage = Base Attack Damage * Damage Reduction%

DEF 100, 0% Penetration -> DEF 100 left,0% Damage Taken -> 0 Damage
DEF 100, 50% Penetration -> DEF 50 left, 50% Damage Taken -> 50 Damage
DEF 100, 100% Penetration -> DEF 0 left, 100% Damage Taken -> 100 Damage
DEF 200, 0% Penetration -> DEF 200 left, 0% Damage Taken -> 0 Damage
DEF 200, 50% Penetration -> DEF 100 left, 0% Damage Taken -> 0 Damage
DEF 200, 100% Penetration -> DEF 0 left, 100% Damage Taken -> 100 Damage

*/