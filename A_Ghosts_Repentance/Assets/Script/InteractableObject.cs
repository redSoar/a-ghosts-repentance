using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class InteractableObject : MonoBehaviour
{

    bool hasCollided = false;
    public TextMeshProUGUI textField;
    bool talking = false;
    [TextArea(3, 3)]
    public string[] dialogueLines;
    public GameObject interact;
    private int currentDialogueIdx;
    bool chatRadius = false;
    public GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        if (textField == null)
        {
            textField = GameObject.FindGameObjectWithTag("Text").GetComponent<TextMeshProUGUI>();
            dialogueBox.SetActive(false);
        }
        textField.text = "";
        interact.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueLines.Length > 0 && chatRadius)
        {
            dialogueBox.SetActive(true);
            Dialogue();
        }

        if (talking == false)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            hasCollided = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            hasCollided = false;
        }
    }

    public void Dialogue()
    {
        if (currentDialogueIdx < dialogueLines.Length)
        {
            textField.text = dialogueLines[currentDialogueIdx];
            currentDialogueIdx++;
        }
        else
        {
            currentDialogueIdx = 0;

        }
    }

}
