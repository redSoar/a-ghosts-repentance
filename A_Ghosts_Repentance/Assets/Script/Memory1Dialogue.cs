using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Memory1Dialogue : MonoBehaviour
{
    private int currentDialogueIdx = 0; // Track current dialogue index
    private bool isWaitingForChoice = false;
    private bool isInteracting = false; // State to track if interaction is in progress
    private bool isDialogueInProgress = false; // Track if dialogue is running
    private bool hasCollided = false;
    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public GameObject interactPrompt;

    private string[] mainDialogueLines = {
        "Bully: Nice day today, huh? Too bad you can’t see the sun in my shadow, shrimp!",
        "Friend: l-leave me alone.......",
        "Bully: What’s that shrimp?! Can’t hear you from allllll the way down there!",
        "Friend: [whimpers]",
        "Friend: h-he-help me out here [MC’s name]...",
        "Bully: Hmm? Oh, look who decided to weasel his way in. You have a problem?",
    };

    private string[] choiceA = { "MC: Uuhhhh... N-nothing, man..." };
    private string[] choiceB = { "MC: ...y-y-yeah stop bullying my friend" };

    private string[] choiceA1 = { "MC: " };
    private string[] choiceB1 = { "MC: " };

    private string[] goodEndingDialogue = {
        "Bully: Aww got a lil tough guy here huh!! What are you going to do about it?!",
        "MC: urk!",
        "Bully: Yeah that’s what I thought.... And just for that, your little shrimp here is gonna get it!",
        "Friend: Wha-huh!!?!? Please, no!",
        "Friend: (runs away)",
        "Bully: Oh what don’t you look at that your shrimp of a “friend” left you! I guess I’ll just have to take it out on you!!",
        "Teacher: What is going on here!",
        "Teacher: [Bully’s name], this is the last time! Let’s go to the principal's office!",
        "Friend: Thanks for sticking up for me. I know that was scary.",
        "MC: Y-yeah. I’m glad you’re safe."
    };

    private string[] badEndingDialogue = {
        "Friend: ..p-p-please...",
        "Bully: Heh.. That’s what I thought... But seeing you pisses me off, so your friend is gonna get it!",
        "Friend: Wha-huh!!?!? Please, no!",
        "MC: *Turns head away*",
        "Bully: Some friend you are! HA! I guess you really have no one shri- huh!?!??",
        "Friend: (runs away)",
        "Bully: GET BACK HE- ... I guess you’ll just have to take his place...."
    };

    void Start()
    {
        ResetState();
    }

    private void ResetState()
    {
        textField.text = "";
        dialogueBox.SetActive(false);
        currentDialogueIdx = 0;
        isInteracting = false;
        isDialogueInProgress = false;
        isWaitingForChoice = false;
    }

    public void Dialogue()
    {
        if (isDialogueInProgress || isWaitingForChoice) return;

        isDialogueInProgress = true;

        if (currentDialogueIdx < mainDialogueLines.Length)
        {
            dialogueBox.SetActive(true);
            textField.text = mainDialogueLines[currentDialogueIdx];
            currentDialogueIdx++;
        }
        else if (currentDialogueIdx >= mainDialogueLines.Length)
        {
            DisplayChoice();
        }

        isDialogueInProgress = false;
    }

    private void DisplayChoice()
    {
        isWaitingForChoice = true;
        textField.text = "1: Uuhhhh... N-nothing, man...\n2: ...y-y-yeah stop bullying my friend";
    }

    void Update()
    {
        if (hasCollided && Input.GetKeyDown(KeyCode.E))
        {
            Dialogue();
        }

        if (isWaitingForChoice)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ProcessChoice(choiceA, false); // Choice A leads to bad ending
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ProcessChoice(choiceB, true); // Choice B leads to good ending
            }
        }
    }

    private void ProcessChoice(string[] chosenDialogue, bool isGoodEnding)
    {
        string[] dialogueLines = chosenDialogue;
        currentDialogueIdx = 0;
        isWaitingForChoice = false;

        if (isGoodEnding)
        {
            dialogueLines = goodEndingDialogue;
        }
        else
        {
            dialogueLines = badEndingDialogue;
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        ResetState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            hasCollided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            EndDialogue();
        }
    }
}
