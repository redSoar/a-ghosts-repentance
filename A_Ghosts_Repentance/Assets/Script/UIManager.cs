using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;
    public bool inUI = false;
    private static UIManager instance;

    // Start is called before the first frame update
    void Start()
    {
        pauseUI.SetActive(false);
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !inUI)
            {
                pauseUI.SetActive(true);
                inUI = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && inUI)
            {
                pauseUI.SetActive(false);
                inUI = false;
            }
            else if (Input.GetKeyDown(KeyCode.Q) && inUI)
            {
                EndGame.GetInstance().QuitGame();
            }
        }
    }

    public static UIManager GetInstance()
    {
        return instance;
    }

}
