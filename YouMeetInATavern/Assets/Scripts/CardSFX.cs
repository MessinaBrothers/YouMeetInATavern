using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSFX : MonoBehaviour {

    public AudioClip introductionClip;
    public AudioClip[] greetingClips;
    public AudioClip[] startConversationClips;

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

    }

    public void PlayIntro() {
        audioSource.clip = introductionClip;
        audioSource.Play();
    }

    public void PlayGreeting() {
        audioSource.clip = greetingClips[Random.Range(0, greetingClips.Length)];
        audioSource.Play();
    }
}
