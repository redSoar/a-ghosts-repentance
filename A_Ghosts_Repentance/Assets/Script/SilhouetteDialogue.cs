using TMPro;
using UnityEngine;

public class SilhouetteDialogue : MonoBehaviour
{
    private int currentDialogueIdx = 0;
    private bool isInteracting = false; // State to track if interaction is in progress
    private bool isDialogueInProgress = false; // Track if dialogue is running
    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public GameObject interactPrompt;
    private bool switchDialogue = false; // signals which dialogue lines to use
    private string[] dialogueLines = new string[4];
    private string[] secondDialogueLines = new string[5];

    void Start()
    {
        dialogueLines[0] = "Hey.";
        dialogueLines[1] = "If you haven't figured it out yet, you're dead.";
        dialogueLines[2] = "You can leave whenever you want, but maybe try looking through those doors over there.";
        dialogueLines[3] = "You might learn something from reliving past memories.";
        ResetState(); // Ensure state is reset at start
    }

    // Reset all interaction state variables
    private void ResetState()
    {
        Debug.Log("Resetting state");
        textField.text = "";
        currentDialogueIdx = 0;
        dialogueBox.SetActive(false);
        isInteracting = false;  // Ensure interaction is false initially
        isDialogueInProgress = false;
    }

    // Dialogue progression method
    public void DialogueStart()
    {
        if (isDialogueInProgress) return; // Prevent multiple dialogue interactions in the same frame

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
        switchDialogue = true;
        ResetState(); // Reset everything when dialogue ends
    }

    // Trigger player interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player collided with interactable object");
            if (!isInteracting) // Start interaction if it's not already in progress
            {
                Debug.Log("Starting interaction from first dialogue line.");
                isInteracting = true;
                if (!switchDialogue)
                {
                    DialogueStart(); // Start the dialogue
                }
                else
                {

                }
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

    // Update method to handle input and dialogue progression
    void Update()
    {
        
    }

}
