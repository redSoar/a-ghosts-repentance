using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{

    private Collider2D collider;
    private ContactFilter2D contactFilter;
    private List<Collider2D> colliderObjects = new List<Collider2D>(1);

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.OverlapCollider(contactFilter, colliderObjects);
        foreach (var o in colliderObjects)
        {
            Collided(o.gameObject);
        }
    }

    protected void Collided(GameObject collideObject)
    {
        //Debug.Log("Collided with " + collideObject.name);
    }
}
