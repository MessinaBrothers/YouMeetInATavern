using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

    public AudioMixerSnapshot main, silent;

    public AudioSource[] tavernAudio, conclusionAudio;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();

        TransitionMain(0);
    }

    public void TransitionSilent(float time) {
        silent.TransitionTo(time);
    }

    public void TransitionMain(float time) {
        main.TransitionTo(time);
    }

    void OnEnable() {
        InputController.gameflowEndBeginTavern += BeginTavern;
        InputController.gameflowStartFinishTavern += FinishTavern;
        InputController.gameflowStartBeginConclusion += StartConclude;
        InputController.gameflowStartFinishConclusion += FinishConclude;
    }

    void OnDisable() {
        InputController.gameflowEndBeginTavern -= BeginTavern;
        InputController.gameflowStartFinishTavern -= FinishTavern;
        InputController.gameflowStartBeginConclusion -= StartConclude;
        InputController.gameflowStartFinishConclusion -= FinishConclude;
    }

    private IEnumerator TransitionSnapshot(AudioMixerSnapshot from, AudioMixerSnapshot to, float time) {
        from.TransitionTo(0);
        yield return null;
        to.TransitionTo(time);
    }

    private void StartConclude() {
        foreach (AudioSource source in conclusionAudio) {
            StartCoroutine(FadeIn(source, data.fadeInTime / data.DEBUG_SPEED_EDITOR));
        }
    }

    private void FinishConclude() {
        foreach (AudioSource source in conclusionAudio) {
            StartCoroutine(FadeOut(source, data.fadeOutTime / data.DEBUG_SPEED_EDITOR));
        }
    }

    private void BeginTavern() {
        foreach (AudioSource source in tavernAudio) {
            StartCoroutine(FadeIn(source, data.fadeInTime / data.DEBUG_SPEED_EDITOR));
        }
    }

    private void FinishTavern() {
        foreach (AudioSource source in tavernAudio) {
            StartCoroutine(FadeOut(source, data.fadeOutTime / data.DEBUG_SPEED_EDITOR));
        }
    }

    private IEnumerator FadeIn(AudioSource source, float time) {
        source.volume = 0f;

        while (source.volume < 1) {
            source.volume += Time.deltaTime / time;
            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource source, float time) {
        source.volume = 1f;
        while (source.volume > 0) {
            source.volume -= Time.deltaTime / time;
            yield return null;
        }
    }
}
