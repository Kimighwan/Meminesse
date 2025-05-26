using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float damage;
    private float travelDistance;
    private float xStartPos;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float damageRadius;

    private bool isGravityOn;
    private bool isHitGround;

    private Rigidbody2D rigid;
    [SerializeField]
    private LayerMask whatIsPlayer;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Transform damagePos;

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.gravityScale = 0f;
        rigid.linearVelocity = transform.right * speed;

        xStartPos = transform.position.x;
        isGravityOn = false;
        isHitGround = false;
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
    }

    public virtual void FixedUpdate()
    {
        if (!isHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, whatIsGround);

            if (damageHit)
            {
                Debug.Log("발사체 피격!");
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

    public void InitProjectile(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        this.damage = damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePos.position, damageRadius);
    }
}
