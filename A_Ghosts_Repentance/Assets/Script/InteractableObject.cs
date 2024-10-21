using UnityEngine;
using TMPro;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
    private bool hasCollided = false;
    private int currentDialogueIdx = 0; // Track current dialogue index
    private bool isWaitingForChoice = false;
    private bool isInteracting = false; // State to track if interaction is in progress
    private bool isDialogueInProgress = false; // Track if dialogue is running

    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public string[] dialogueLines; // Dialogue lines array
    public string[] choiceA;
    public string[] choiceB;
    public bool is_choice = false;
    public string choice1;
    public string choice2;

    void Start()
    {
        ResetState(); // Ensure state is reset at start
    }

    // Reset all interaction state variables
    private void ResetState()
    {
        Debug.Log("Resetting state");
        textField.text = "";
        dialogueBox.SetActive(false);
        currentDialogueIdx = 0; // Ensure index starts at the first element
        isInteracting = false;  // Ensure interaction is false initially
        isDialogueInProgress = false;
        isWaitingForChoice = false;
    }

    // Dialogue progression method
    public void Dialogue()
    {
        if (isDialogueInProgress) return; // Prevent multiple dialogue interactions in the same frame
        
        isDialogueInProgress = true; // Mark dialogue as in progress

        if (is_choice && !isWaitingForChoice)
        {
            DisplayChoice(); // Call method to show choices
        }
        else if (!isWaitingForChoice && currentDialogueIdx < dialogueLines.Length)
        {
            dialogueBox.SetActive(true);
            Debug.Log("Displaying element " + currentDialogueIdx + ": " + dialogueLines[currentDialogueIdx]); // Debug to track dialogue
            textField.text = dialogueLines[currentDialogueIdx]; // Show current dialogue
            textField.ForceMeshUpdate();
            currentDialogueIdx++; // Move to the next line
        }
        else if (currentDialogueIdx >= dialogueLines.Length)
        {
            EndDialogue(); // End if dialogue is finished
        }

        isDialogueInProgress = false; // Allow dialogue progression in next frame
    }

    // End dialogue and reset everything
    private void EndDialogue()
    {
        Debug.Log("End of dialogue");
        ResetState(); // Reset everything when dialogue ends
        StopCoroutine("ListenForInput"); // Stop listening for input
    }

    // Trigger player interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            hasCollided = true;
            Debug.Log("Player collided with interactable object");
            if (!isInteracting) // Start interaction if it's not already in progress
            {
                Debug.Log("Starting interaction from first dialogue line.");
                currentDialogueIdx = 0; // Ensure dialogue starts from the first element
                isInteracting = true;
                StartCoroutine(ListenForInput());
            }
        }
    }

    // Reset when player leaves
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            hasCollided = false;
            Debug.Log("Player left the interaction zone, resetting dialogue.");
            EndDialogue();
        }
    }

    // Coroutine to listen for input while the player is in range
    private IEnumerator ListenForInput()
    {
        while (isInteracting) // Keep listening while interaction is active
        {
            if (Input.GetKeyDown(KeyCode.E)) // Listen for the 'E' key press
            {
                Debug.Log("Interacting with object");
                Dialogue(); // Call the Dialogue method
            }
            yield return null; // Wait for next frame
        }
    }

    // Display choices and wait for player input
    private void DisplayChoice()
    {
        isWaitingForChoice = true;
        textField.text = choice1 + "\n" + choice2; // Display choice options
        Debug.Log("Displaying choices: " + choice1 + " | " + choice2);
        Choice();
    }

    // Choice method to handle the player's input
    private void Choice()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // If player presses '1' for choice A
        {
            ProcessChoice(choiceA);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // If player presses '2' for choice B
        {
            ProcessChoice(choiceB);
        }
    }

    // Process the player's choice
    private void ProcessChoice(string[] chosenDialogue)
    {
        dialogueLines = chosenDialogue; // Set dialogue lines to the chosen branch
        currentDialogueIdx = 0;         // Reset dialogue index for new choice dialogue branch
        is_choice = false;              // No more choices, continue dialogue
        isWaitingForChoice = false;     // Choice has been made, exit choice mode
        Debug.Log("Choice made, continuing dialogue");
        Dialogue();                     // Continue dialogue based on choice
    }
}
