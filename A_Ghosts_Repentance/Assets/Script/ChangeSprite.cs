using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSprite : MonoBehaviour
{
    public Sprite front;
    public Sprite left;
    public Sprite right;
    public Sprite back;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            // changes sprite based on key input
            if (Input.GetKey(KeyCode.A))
            {
                sr.sprite = left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                sr.sprite = right;
            }

            if (Input.GetKey(KeyCode.W))
            {
                sr.sprite = back;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                sr.sprite = front;
            }
        }
    }
}
