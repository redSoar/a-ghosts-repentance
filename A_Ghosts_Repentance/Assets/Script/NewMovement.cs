using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Speed
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            move.x = -1; // Move left
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move.x = 1;  // Move right
        }

        if (Input.GetKey(KeyCode.W))
        {
            move.y = 1;  // Move up
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move.y = -1; // Move down
        }

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}