using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float MoveInputDirection { get; private set; }
    public float MoveSpeed { get; private set; }

    void Start()
    {
        MoveSpeed = 10f;
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveInputDirection = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        rigid.linearVelocityX = MoveSpeed * MoveInputDirection;
    }
}
