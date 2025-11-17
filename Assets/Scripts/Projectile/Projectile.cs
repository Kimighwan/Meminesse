using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Projectile : MonoBehaviour
{
    protected float speed;
    protected float damage;
    protected float travelDistance;
    protected float xStartPos;
    [SerializeField]
    protected float gravity;
    [SerializeField]
    protected float damageRadius;

    protected bool isGravityOn;
    protected bool isHitGround;
    protected bool isHitPlayer;

    protected Vector2 targetPos;
    protected Vector2 fireDir;

    protected Rigidbody2D rigid;
    [SerializeField]
    protected LayerMask whatIsPlayer;
    [SerializeField]
    protected LayerMask whatIsGround;
    [SerializeField]
    protected Transform damagePos;

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.gravityScale = 0f;

        fireDir = targetPos - new Vector2(transform.position.x, transform.position.y);
        fireDir.Normalize();
        rigid.linearVelocity = fireDir * speed;

        xStartPos = transform.position.x;
        isGravityOn = false;
        isHitGround = false;
        isHitPlayer = false;
    }

    public virtual void Update()
    {
        if (!isHitGround)
        {
            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rigid.linearVelocityY, rigid.linearVelocityX) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }

        if(isHitGround || isHitPlayer)
        {
            rigid.simulated = false;
            Destroy(gameObject, 2f);
        }
    }

    public virtual void FixedUpdate()
    {
        if (!isHitGround && !isHitPlayer)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                if(damageHit.CompareTag("Player"))
                {
                    Debug.Log("Player Hit by Projectile");
                    damageHit.GetComponent<PlayerController>().Damaged(damage, transform.position);
                    isHitPlayer = true;
                }
            }

            if (groundHit)
            {
                isHitGround = true;
                rigid.gravityScale = 0f;
                rigid.linearVelocity = Vector2.zero;
            }

            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rigid.gravityScale = gravity;
            }
        }
    }

    public void InitProjectile(float speed, float travelDistance, float damage, Vector2 targetPos)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        this.damage = damage;
        this.targetPos = targetPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePos.position, damageRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<PlayerController>();
        //player.Damaged(damage, entity.gameObject.transform.position);
    }
}
