using UnityEngine;

public class VoidbornGoddess_Spell2 : Projectile
{
    public override void Start()
    {
        base.Start();

        InitProjectile(10f, 100f, 20f, Vector2.right, entity);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isHitGround || isHitPlayer)
        {
            GetComponent<Animator>().SetBool("impact", true);
            rigid.simulated = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
