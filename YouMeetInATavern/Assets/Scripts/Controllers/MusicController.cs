using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour {

    public AudioMixerSnapshot main, silent, conclusion;

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

    public void TransitionConclusion(float time) {
        conclusion.TransitionTo(time);
    }

    void OnEnable() {
        InputController.startDayEventHandler += FadeInMusic;
        InputController.startConcludeScenarioEventHandler += StartConclude;
    }

    void OnDisable() {
        InputController.startDayEventHandler -= FadeInMusic;
        InputController.startConcludeScenarioEventHandler -= StartConclude;
    }

    private void FadeInMusic() {
        StartCoroutine(TransitionSnapshot(silent, main, data.fadeInTime));
    }

    private IEnumerator TransitionSnapshot(AudioMixerSnapshot from, AudioMixerSnapshot to, float time) {
        from.TransitionTo(0);
        yield return null;
        to.TransitionTo(time);
    }

    private void StartConclude() {
        TransitionConclusion(data.fadeInTime);
    }
}
