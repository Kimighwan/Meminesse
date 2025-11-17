using UnityEngine;

public class BulletA : MonoBehaviour
{
    protected float speed;
    protected float damage;
    [SerializeField] protected float damageRadius;
    [SerializeField] protected Transform damagePos;

    protected bool isHitGround;
    protected bool isHitPlayer;

    protected Rigidbody2D rigid;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsGround;

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        isHitGround = false;
        isHitPlayer = false;
    }

    public virtual void FixedUpdate()
    {
        if (!isHitGround && !isHitPlayer)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                damageHit.GetComponent<PlayerController>().Damaged(damage, transform.position);
                isHitPlayer = true;
            }

            if (groundHit)
            {
                isHitGround = true;
                rigid.gravityScale = 0f;
                rigid.linearVelocity = Vector2.zero;
            }
        }

        if (isHitGround || isHitPlayer)
        {
            GetComponent<Animator>().SetBool("impact", true);
            rigid.simulated = false;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePos.position, damageRadius);
    }
}
