using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public int facingDirection {  get; private set; }

    public Rigidbody2D rigid { get; private set; }
    public Animation anim { get; private set; }
    public GameObject aliveGO { get; private set; }


    public virtual void Start()
    {
        
    }
}
