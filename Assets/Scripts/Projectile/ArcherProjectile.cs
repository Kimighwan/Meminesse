using UnityEngine;

public class ArcherProjectile : Projectile
{
    //private Collider2D coll;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Start()
    {
        base.Start();

        //coll = GetComponent<Collider2D>();
    }

    public override void Update()
    {
        base.Update();
        
        Destroy(gameObject, 3f);
        //if (isHitGround)
        //{
        //    rigid.simulated = false;
        //    coll.isTrigger = true;
        //}
    }
}
