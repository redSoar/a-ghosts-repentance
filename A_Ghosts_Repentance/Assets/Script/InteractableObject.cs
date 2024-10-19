using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    private bool hasCollided = false;
    private int currentDialogueIdx = 0;
    private bool isWaitingForChoice = false;

    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public string[] dialogueLines;
    public string[] choiceA;
    public string[] choiceB;
    public bool is_choice;
    public string choice1;
    public string choice2;

    // Start is called before the first frame update
    void Start()
    {
        if (textField == null)
        {
            textField = GameObject.FindGameObjectWithTag("Text").GetComponent<TextMeshProUGUI>();
        }
        dialogueBox.SetActive(false);
        textField.text = "";
    }

    // Dialogue progression method
    public void Dialogue()
    {
        if (is_choice && !isWaitingForChoice) // Check if this is a choice moment
        {
            DisplayChoice(); // Call the method to show choices
        }
        else if (!isWaitingForChoice && dialogueLines.Length > 0) // Normal dialogue
        {
            if (currentDialogueIdx < dialogueLines.Length)
            {
                dialogueBox.SetActive(true);
                textField.text = dialogueLines[currentDialogueIdx]; // Show current dialogue
                currentDialogueIdx++;
            }
            else
            {
                EndDialogue(); // End if dialogue is finished
            }
        }
    }

    // End dialogue and reset everything
    private void EndDialogue()
    {
        currentDialogueIdx = 0;
        dialogueBox.SetActive(false);
        textField.text = "";
        isWaitingForChoice = false; // Reset choice flag
    }

    // Trigger player interaction
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasCollided = true;
            Debug.Log("i vant to keel myself");
        }
    }

    // Reset when player leaves
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasCollided = false;
            EndDialogue();
        }
    }

    // Display choices and wait for player input
    private void DisplayChoice()
    {
        isWaitingForChoice = true;
        textField.text = choice1 + "\n" +choice2; // Display choice options
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
        currentDialogueIdx = 0;         // Reset dialogue index for new choice
        is_choice = false;              // No more choices, continue dialogue
        isWaitingForChoice = false;     // Choice has been made, exit choice mode
        Dialogue();                     // Continue dialogue based on choice
    }

    // Update method to continuously check for input
    private void Update()
    {
        if (hasCollided && Input.GetKeyDown(KeyCode.E)) // Begin dialogue interaction
        {
            Dialogue();
        }

        if (isWaitingForChoice) // Check for player input during choice
        {
            Choice();
        }
    }
}
