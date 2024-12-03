using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurgMusicFudgery : MonoBehaviour
{
    AudioSource audioSource;
    public float[] pitches = new float[5];
    float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 5f)
        {
            audioSource.pitch = pitches[Random.Range(0, 5)];
            timePassed = 0f;
        }
    }
}
