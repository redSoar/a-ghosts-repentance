using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryMusicFudgery : MonoBehaviour
{
    AudioSource audioSource;
    bool weInDialogue = false;
    float initPitch;
    public float dialoguePitch;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initPitch = audioSource.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying && !weInDialogue)
        {
            audioSource.pitch = dialoguePitch;
            weInDialogue = true;
        }
        else if (!DialogueManager.GetInstance().dialogueIsPlaying && weInDialogue)
        {
            audioSource.pitch = initPitch;
            weInDialogue = false;
        }
    }
}
