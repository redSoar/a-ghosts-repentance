using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsDialogue : MonoBehaviour
{
    static bool currentlyInteracting = false;
    private string sceneName = SceneManager.GetActiveScene().name;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName == "HubTest")
        {
            GameObject silhouette = GameObject.FindGameObjectWithTag("Silhouette");
            currentlyInteracting = silhouette.GetComponent<SilhouetteDialogue>();
        }
    }
}
