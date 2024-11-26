using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    bool hasCollided = false;
    public string nextScene;
    public GameObject interactPrompt;
    private AudioSource audioSource;
    public AudioClip doorSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = doorSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasCollided)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                audioSource.Play();
                Invoke("ChangeTheScene", 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            hasCollided = true;
            interactPrompt.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            hasCollided = false;
            interactPrompt.SetActive(false);
        }
    }

    private void ChangeTheScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
