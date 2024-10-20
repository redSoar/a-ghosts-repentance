using System.Collections;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    private bool canInteract = false;
    private GameObject interactableObject;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E)) // If the player is near an interactable object and presses 'E'
        {
            interactableObject.GetComponent<InteractableObject>().Dialogue();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Interactable") || collision.collider.CompareTag("Door"))
        {
            interactableObject = collision.gameObject;
            canInteract = true;

            if (collision.collider.CompareTag("Interactable"))
            {
                Debug.Log("Press 'E' to interact");
            }
            else if (collision.collider.CompareTag("Door"))
            {
                Debug.Log("Press 'M' to go through door");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == interactableObject)
        {
            canInteract = false;
            interactableObject = null;
        }
    }
}
