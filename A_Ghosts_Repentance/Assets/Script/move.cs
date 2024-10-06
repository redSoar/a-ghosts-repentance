using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Speed of the player movement
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Get input from A and D keys for horizontal movement on X-axis
        float moveX = Input.GetAxis("Horizontal"); // A and D keys (or Left and Right arrows)

        // Get input from W and S keys for vertical movement on Y-axis
        float moveY = Input.GetAxis("Vertical");   // W and S keys (or Up and Down arrows)

        // Create a movement vector based on input
        Vector3 move = new Vector3(moveX, moveY, 0);

        // Move the player object based on the input
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}


