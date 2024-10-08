using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    private bool interacted = false;

    protected void OnCollision(GameObject collideObject)
    {
        if (Input.GetKey(KeyCode.E))
        {
            OnInteract();
        }
    }


    protected virtual void OnInteract()
    {
        if (!interacted)
        {
            interacted = true;
            Debug.Log("Interacted with" + name);
        }
    }

}
