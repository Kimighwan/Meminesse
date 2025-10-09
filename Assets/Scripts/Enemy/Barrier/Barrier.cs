using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] bool higherDefence;
    float hp = 100;
    private float defence;

    void Start()
    {
        if (higherDefence)
            defence = 200;
        else
            defence = 100;
    }

    public void Damaged(float damage, float defIgnore)
    {
        float realDefence = defence * (1 - defIgnore);
        float damageReduction = 1 - (realDefence / 100);
        damage *= damageReduction;

        if (damage <= 0)
            damage = 1;

        hp -= damage;

        if (hp <= 0)
        {
            Destroy(this);
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