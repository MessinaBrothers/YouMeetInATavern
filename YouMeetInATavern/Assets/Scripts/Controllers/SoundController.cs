using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public AudioSource source;

    public AudioClip[] cardIntroSwooshClips, conclusionBackgroundClips, buttonClickClips, tavernClickClips, grandfatherclockClickClips;

    public GameObject grandfatherclock;

    private GameData data;

    void Awake() {
        data = FindObjectOfType<GameData>();
    }

    void OnEnable() {
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.npcIntroStartEventHandler += HandleIntroduction;
        //InputController.continueDialogueEventHandler += HandleIntroductionEnd;
        InputController.dialogueCardCreatedEventHandler += HandleCardCreated;
        InputController.conclusionBackgroundClicked += HandleConclusionBackgroundClick;
        InputController.questionEventHandler += PlayButtonClick;
        InputController.endDialogueEventHandler += PlayButtonClick;
        InputController.endDayEarlyEventHandler += PlayButtonClick;
        InputController.deckClickedEventHander += PlayButtonClick;
        InputController.deckClosedEventHander += PlayButtonClick;
        InputController.checkAnswersEventHandler += PlayButtonClick;
        InputController.tavernClicked += PlayTavernClick;
        InputController.tutorialScreenClickedEventHandler += PlayButtonClick;
        InputController.itemClicked += PlayItemClick;
    }

    void OnDisable() {
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.npcIntroStartEventHandler -= HandleIntroduction;
        //InputController.continueDialogueEventHandler -= HandleIntroductionEnd;
        InputController.dialogueCardCreatedEventHandler -= HandleCardCreated;
        InputController.conclusionBackgroundClicked -= HandleConclusionBackgroundClick;
        InputController.questionEventHandler -= PlayButtonClick;
        InputController.endDialogueEventHandler -= PlayButtonClick;
        InputController.endDayEarlyEventHandler -= PlayButtonClick;
        InputController.deckClickedEventHander -= PlayButtonClick;
        InputController.deckClosedEventHander -= PlayButtonClick;
        InputController.checkAnswersEventHandler -= PlayButtonClick;
        InputController.tavernClicked -= PlayTavernClick;
        InputController.tutorialScreenClickedEventHandler -= PlayButtonClick;
        InputController.itemClicked -= PlayItemClick;
    }

    private void HandleIntroduction(GameObject card) {
        Play(cardIntroSwooshClips);
    }

    private void HandleIntroductionEnd(GameObject card) {
        Play(cardIntroSwooshClips);
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.TAVERN) {
            card.GetComponent<CardSFX>().PlayBeginConverse();
        } else if (data.gameMode == GameData.GameMode.CONVERSE) {
            card.GetComponent<CardSFX>().PlayGreeting();
        }
    }

    private void HandleCardCreated(GameObject card) {
        Play(cardIntroSwooshClips);
    }

    private void HandleConclusionBackgroundClick(Vector3 position) {
        Play(conclusionBackgroundClips);
    }

    private void PlayButtonClick() {
        Play(buttonClickClips);
    }

    private void PlayButtonClick(GameObject card) {
        Play(buttonClickClips);
    }

    private void PlayButtonClick(GameObject currentScreen, GameObject nextScreen) {
        Play(buttonClickClips);
    }

    private void PlayButtonClick(Dialogue question) {
        Play(buttonClickClips);
    }

    private void PlayTavernClick(Vector3 position) {
        Play(tavernClickClips);
    }

    private void PlayItemClick(GameObject item, Vector3 position) {
        if (item == grandfatherclock) {
            Play(grandfatherclockClickClips);
        }
    }

    private void Play(AudioClip[] clips) {
        source.PlayOneShot(GetRandomClip(clips));
    }

    private AudioClip GetRandomClip(AudioClip[] clips) {
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

}
