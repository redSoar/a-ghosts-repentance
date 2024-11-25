using System.Collections;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    
    private bool canInteract = false;
    private GameObject interactableObject;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E) && interactableObject.CompareTag("Interactable")) // If the player is near an interactable object and presses 'E'
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
                interactableObject.GetComponent<InteractableObject>().interactPrompt.SetActive(true);
            }
            else if (collision.collider.CompareTag("Door"))
            {
                interactableObject.GetComponent<ChangeScene>().interactPrompt.SetActive(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == interactableObject)
        {
            canInteract = false;
            if (collision.collider.CompareTag("Interactable"))
            {
                Debug.Log("interacting?");
                interactableObject.GetComponent<InteractableObject>().interactPrompt.SetActive(false);
            }
            else if (collision.collider.CompareTag("Door"))
            {
                interactableObject.GetComponent<ChangeScene>().interactPrompt.SetActive(false);
            }
            interactableObject = null;
        }
    }
    
}
