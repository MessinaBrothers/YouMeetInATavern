using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSFX : MonoBehaviour {

    public AudioClip introductionClip;
    public AudioClip[] greetingClips;
    public AudioClip[] startConversationClips;
    public AudioClip[] goodbyeClips;

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
        PlayRandomClip(greetingClips);
    }

    public void PlayBeginConverse() {
        PlayRandomClip(startConversationClips);
    }

    public void PlayGoodbye() {
        PlayRandomClip(goodbyeClips);
    }

    private void PlayRandomClip(AudioClip[] clips) {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
