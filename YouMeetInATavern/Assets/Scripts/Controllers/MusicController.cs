using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

    public AudioMixerSnapshot main, silent;

    //private AudioSource audioSource;

    void Awake() {
        //audioSource = GetComponent<AudioSource>();
        TransitionMain(0);
    }

    public void TransitionSilent(float time) {
        silent.TransitionTo(time);
    }

    public void TransitionMain(float time) {
        main.TransitionTo(time);
    }
}
