using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSFX : MonoBehaviour {

    public AudioClip introductionClip;

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
}
