using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Interactable")
        {
            Debug.Log("Press 'E' to interact");
        } 
        else if (collision.collider.tag == "Door")
        {
            Debug.Log("Press 'M' to go through door");
        }
    }
}
