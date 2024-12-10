using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoors : MonoBehaviour
{
    public GameObject[] doors;

    AudioSource audioSource;
    public AudioClip voiceClip;
    public AudioClip doorClip;
    bool hasCollided = false;
    bool unlockedDoors = false;
    int doorIndex = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voiceClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            audioSource.clip = voiceClip;
            audioSource.Play();

            if (hasCollided && !unlockedDoors)
            {
                unlockedDoors = true;
                Invoke("MakeDoorAppear", 5);
            }
        }
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
            hasCollided = false;
        }
    }

    void MakeDoorAppear()
    {
        if (doorIndex < doors.Length)
        {
            audioSource.clip = doorClip;
            audioSource.Play();
            doors[doorIndex].SetActive(true);
            doorIndex++;
            Invoke("MakeDoorAppear", 1);
        }
        else
        {
            audioSource.clip = voiceClip;
        }
    }
}
