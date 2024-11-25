using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public InputAction playerControls;
    public InputAction interact;
    private bool interactPressed = false;
    Vector2 moveDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls.Enable();
        interact.Enable(); // Enable the interact input action
        interact.performed += InteractPerformed; // Subscribe to the input event
        interact.canceled += InteractCanceled;  // Optional: Handle cancellations
    }

    private void OnDisable()
    {
        playerControls.Disable();
        interact.Disable();
        interact.performed -= InteractPerformed;
        interact.canceled -= InteractCanceled;
    }

    private void Update()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying && !UIManager.GetInstance().inUI)
        {
            moveDirection = playerControls.ReadValue<Vector2>();
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void InteractPerformed(InputAction.CallbackContext context)
    {
        interactPressed = true;
    }

    private void InteractCanceled(InputAction.CallbackContext context)
    {
        interactPressed = false;
    }

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false; // Reset interact after checking
        return result;
    }
}