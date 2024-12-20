using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject dialogueChoicesPanel; // Reference to DialogueChoices object
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject[] choices; // Array of your 6 choice buttons

    private TextMeshProUGUI[] choicesText;
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    private AudioSource audioSource;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;
    private bool canContinueToNextLine = false;
    private Coroutine displayLineCoroutine;
    private const string AUDIO_TAG = "audio";
    private DialogueVariables dialogueVariables;
    private PlayerInput playerInput;
    private bool waitingForChoiceInput = false;
    private bool isTyping = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
            Destroy(gameObject);
            return;
        }
        instance = this;

        playerInput = FindObjectOfType<PlayerInput>();
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);

        audioSource = this.gameObject.AddComponent<AudioSource>();
        currentAudioInfo = defaultAudioInfo;

        // Initialize choices text array
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        // Validate components
        ValidateComponents();
    }

    private void ValidateComponents()
    {
        if (dialoguePanel == null)
            Debug.LogError("DialoguePanel not assigned in inspector!");
        if (dialogueChoicesPanel == null)
            Debug.LogError("DialogueChoicesPanel not assigned in inspector!");
        if (dialogueText == null)
            Debug.LogError("DialogueText not assigned in inspector!");
        if (choices == null || choices.Length == 0)
            Debug.LogError("Choices array not set in inspector!");

        // Validate each choice button
        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == null)
                Debug.LogError($"Choice button {i + 1} not assigned in inspector!");
            if (choicesText[i] == null)
                Debug.LogError($"Choice {i + 1} is missing TextMeshProUGUI component!");
        }
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueChoicesPanel.SetActive(false);
        InitializeAudioInfoDictionary();
        HideChoices();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach (DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if (audioInfo != null)
        {
            this.currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;

        if (FindObjectOfType<PlayerMovement>().GetInteractPressed())
        {
            if (isTyping)
            {
                CompleteTyping();
                return;
            }
            if (canContinueToNextLine) {

                if (!waitingForChoiceInput && currentStory.currentChoices.Count == 0)
                {
                    ContinueStory();
                }
                else if (currentStory.currentChoices.Count > 0)
                {
                    DisplayChoices();
                }
            }
        }
    }

    private void CompleteTyping()
    {
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }
        dialogueText.maxVisibleCharacters = dialogueText.text.Length;
        isTyping = false;
        canContinueToNextLine = true;

        /*
        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        */
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        waitingForChoiceInput = false;

        dialogueVariables.StartListening(currentStory);
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        SetCurrentAudioInfo(defaultAudioInfo.id);
        waitingForChoiceInput = false;
        isTyping = false;
        HideChoices();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }

            HideChoices();
            dialogueText.text = ""; // Clear text when continuing story

            string nextLine = currentStory.Continue();
            if (string.IsNullOrEmpty(nextLine) && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else
            {
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
        }
        else if (currentStory.currentChoices.Count == 0)
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        HideChoices();
        isTyping = true;
        canContinueToNextLine = false;

        AudioClip voiceClip = currentAudioInfo.voiceSoundClip;
        
        audioSource.PlayOneShot(voiceClip);
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
        canContinueToNextLine = true;

        /*
        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
        */
    }

    private void DisplayChoices()
    {
        if (!isTyping)//Issue
        {
            // Clear the dialogue text when choices appear
            dialogueText.text = "";

            List<Choice> currentChoices = currentStory.currentChoices;
            waitingForChoiceInput = currentChoices.Count > 0;

            if (currentChoices.Count > 0)
            {
                dialogueChoicesPanel.SetActive(true);
            }

            // Safety check
            if (currentChoices.Count > choices.Length)
            {
                Debug.LogWarning($"More choices given ({currentChoices.Count}) than UI can support ({choices.Length})");
            }

            int index = 0;
            // Enable and initialize the choices up to the amount of choices for this line of dialogue
            foreach (Choice choice in currentChoices)
            {
                if (index < choices.Length)
                {
                    choices[index].gameObject.SetActive(true);
                    choicesText[index].text = choice.text;
                    index++;
                }
            }
            // Hide the remaining choices
            for (int i = index; i < choices.Length; i++)
            {
                choices[i].gameObject.SetActive(false);
            }

            if (currentChoices.Count > 0)
            {
                StartCoroutine(SelectFirstChoice());
            }
        }
    }

    private void HideChoices()
    {
        dialogueChoicesPanel.SetActive(false);
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        yield return new WaitForEndOfFrame();

        if (choices.Length > 0 && choices[0] != null && choices[0].gameObject.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (waitingForChoiceInput && choiceIndex >= 0 && choiceIndex < currentStory.currentChoices.Count)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            waitingForChoiceInput = false;
            ContinueStory();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }
}