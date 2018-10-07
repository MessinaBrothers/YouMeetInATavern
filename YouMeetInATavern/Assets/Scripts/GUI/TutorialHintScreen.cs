using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialHintScreen : MonoBehaviour, IPointerClickHandler {

    public GameObject nextHintScreen;

    public void OnPointerClick(PointerEventData eventData) {
        InputController.ClickTutorialScreen(gameObject, nextHintScreen);
    }
}
