using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialHintScreen : MonoBehaviour, IPointerClickHandler {

    public GameObject nextHintScreen;

    private FadeText textFader;

    void Awake() {
        textFader = GetComponentInChildren<Text>().gameObject.AddComponent<FadeText>();
        textFader.fadeTime = FindObjectOfType<GameData>().tutorialTextFadeTime;
    }

    void OnEnable() {
        textFader.FadeOut();
    }

    public void OnPointerClick(PointerEventData eventData) {
        InputController.ClickTutorialScreen(gameObject, nextHintScreen);
        textFader.FadeIn();
    }
}
