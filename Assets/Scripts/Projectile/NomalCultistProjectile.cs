using UnityEngine;

public class NomalCultistProjectile : Projectile
{
    public override void Start()
    {
        base.Start();

        rigid.linearVelocity = transform.right * -1 * speed;
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
