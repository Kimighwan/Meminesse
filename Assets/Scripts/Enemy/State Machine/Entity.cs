using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public Animator anim { get; private set; }
    public Rigidbody2D rigid {  get; private set; }
    public GameObject aliveGO { get; private set; }

    public virtual void Start()
    {
        aliveGO = transform.Find("Alive").gameObject;
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        stateMachine = new FiniteStateMachine();
    }

    private void Update()
    {
        stateMachine.currentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
}
