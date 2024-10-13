using UnityEngine;
using TMPro;

public class InteractableObject : MonoBehaviour
{
    private bool hasCollided = false;
    private int currentDialogueIdx = 0;

    public TextMeshProUGUI textField;
    public GameObject dialogueBox;
    public string[] dialogueLines;

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

    public void Dialogue()
    {
        if (dialogueLines.Length > 0)
        {
            if (currentDialogueIdx < dialogueLines.Length)
            {
                dialogueBox.SetActive(true);
                textField.text = dialogueLines[currentDialogueIdx];
                currentDialogueIdx++;
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void EndDialogue()
    {
        currentDialogueIdx = 0;
        dialogueBox.SetActive(false); 
        textField.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hasCollided = false;
            EndDialogue(); 
        }
    }
}
