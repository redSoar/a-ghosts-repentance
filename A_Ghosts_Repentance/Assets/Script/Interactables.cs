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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable") || collision.CompareTag("Door"))
        {
            interactableObject = collision.gameObject;
            canInteract = true;

            if (collision.CompareTag("Interactable"))
            {
                Debug.Log("Press 'E' to interact");
            }
            else if (collision.CompareTag("Door"))
            {
                Debug.Log("Press 'M' to go through door");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == interactableObject)
        {
            canInteract = false;
            interactableObject = null;
        }
    }
}
