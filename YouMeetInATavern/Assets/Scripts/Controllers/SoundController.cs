using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource source;

    public AudioClip[] cardIntroSwooshClips;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.npcIntroStartEventHandler += HandleIntroduction;
        InputController.stopConverseEventHandler += HandleIntroductionEnd;
        InputController.dialogueCardCreatedEventHandler += HandleCardCreated;
    }

    void OnDisable() {
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.npcIntroStartEventHandler -= HandleIntroduction;
        InputController.stopConverseEventHandler -= HandleIntroductionEnd;
        InputController.dialogueCardCreatedEventHandler -= HandleCardCreated;
    }

    private void HandleIntroduction(GameObject card) {
        source.PlayOneShot(GetRandomClip(cardIntroSwooshClips));
    }

    private void HandleIntroductionEnd(GameObject card) {
        source.PlayOneShot(GetRandomClip(cardIntroSwooshClips));
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.TAVERN) {
            card.GetComponent<CardSFX>().PlayBeginConverse();
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void HandleCardCreated(GameObject card) {
        source.PlayOneShot(GetRandomClip(cardIntroSwooshClips));
    }

    private AudioClip GetRandomClip(AudioClip[] clips) {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
