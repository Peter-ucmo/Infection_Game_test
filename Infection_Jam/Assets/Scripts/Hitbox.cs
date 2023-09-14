using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour
{
    BoxCollider2D collider;

    public GameObject Instigator { get; set; }
    public int Damage { get; set; }

    public UnityColliderEvent onHitboxHit = new UnityColliderEvent();


    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;

        Gizmos.DrawLineList(new Vector3[8]
        {
            transform.position + new Vector3(-collider.size.x / 2, -collider.size.y / 2, 0),
            transform.position + new Vector3(collider.size.x / 2, -collider.size.y / 2, 0),
            transform.position + new Vector3(collider.size.x / 2, -collider.size.y / 2, 0),
            transform.position + new Vector3(collider.size.x / 2, collider.size.y / 2, 0),
            transform.position + new Vector3(collider.size.x / 2, collider.size.y / 2, 0),
            transform.position + new Vector3(-collider.size.x / 2, collider.size.y / 2, 0),
            transform.position + new Vector3(-collider.size.x / 2, collider.size.y / 2, 0),
            transform.position + new Vector3(-collider.size.x / 2, -collider.size.y / 2, 0)
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onHitboxHit.Invoke(collision);
    }
}

public class UnityColliderEvent : UnityEvent<Collider2D>
{

}
