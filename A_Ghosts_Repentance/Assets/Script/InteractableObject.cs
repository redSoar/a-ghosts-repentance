using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    private int currentDialogueIdx = 0; // Track current dialogue index
    private bool isWaitingForChoice = false;
    private bool isInteracting = false; // State to track if interaction is in progress
    private bool isDialogueInProgress = false; // Track if dialogue is running
    private bool hasCollided = false;
    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public string[] dialogueLines; // Dialogue lines array
    public string[] choiceA;
    public string[] choiceB;
    public bool is_choice = false;
    public string choice1;
    public string choice2;
    public GameObject interactPrompt;

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
        if (isDialogueInProgress || isWaitingForChoice) return; // Prevent multiple dialogue interactions in the same frame

        isDialogueInProgress = true; // Mark dialogue as in progress

        // Show dialogue lines until the last one is reached
        if (currentDialogueIdx < dialogueLines.Length)
        {
            dialogueBox.SetActive(true);
            Debug.Log("Displaying element " + currentDialogueIdx + ": " + dialogueLines[currentDialogueIdx]);
            textField.text = dialogueLines[currentDialogueIdx]; // Show current dialogue
            textField.ForceMeshUpdate();
            currentDialogueIdx++; // Move to the next line
        }
        else if (currentDialogueIdx >= dialogueLines.Length && is_choice) // When all lines are displayed, show choices
        {
            DisplayChoice(); // Call method to show choices
        }
        else
        {
            EndDialogue(); // End dialogue if no more lines and no choices
        }

        isDialogueInProgress = false; // Allow dialogue progression in the next frame
    }

    // End dialogue and reset everything
    private void EndDialogue()
    {
        Debug.Log("End of dialogue");
        ResetState(); // Reset everything when dialogue ends
    }

    // Trigger player interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            hasCollided = true;
            Debug.Log("Player collided with interactable object");
            if (!isInteracting) // Start interaction if it's not already in progress
            {
                Debug.Log("Starting interaction from first dialogue line.");
                currentDialogueIdx = 0; // Ensure dialogue starts from the first element
                isInteracting = true;
                Dialogue(); // Start the dialogue
            }
        }
    }

    // Reset when player leaves
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player left the interaction zone, resetting dialogue.");
            EndDialogue();
        }
    }

    // Display choices and wait for player input
    private void DisplayChoice()
    {
        Debug.Log("Displaying choices: " + choice1 + " | " + choice2);
        isWaitingForChoice = true; // Set flag indicating we're waiting for a choice
        textField.text = choice1 + "\n" + choice2; // Display choice options
    }

    // Update method to handle input and dialogue progression
    void Update()
    {
        if (hasCollided && Input.GetKeyDown(KeyCode.E)) // Check if the player presses 'E' to interact
        {
            Debug.Log("Interacting with object (input detected)");
            if (currentDialogueIdx == 0)
            {
                Debug.Log("First interaction. Starting from first dialogue line.");
            }
            Dialogue(); // Start dialogue
        }

        // Continuously check for input if waiting for a choice
        if (isWaitingForChoice)
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
    }

    // Process the player's choice
    private void ProcessChoice(string[] chosenDialogue)
    {
        Debug.Log("Choice made, continuing dialogue");
        dialogueLines = chosenDialogue; // Set dialogue lines to the chosen branch
        currentDialogueIdx = 0;         // Reset dialogue index for new choice dialogue branch
        isWaitingForChoice = false;     // Choice has been made, exit choice mode
        is_choice = false;              // Turn off choices after selection
        Dialogue();                     // Continue dialogue based on choice
    }
}
