using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckListItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private CardData cardData;
    private int index;

    public void Load(CardData cardData, int index) {
        this.cardData = cardData;
        this.index = index;

        SetText(cardData.name);
    }

    private void SetText(string text) {
        GetComponentInChildren<Text>().text = text;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        InputController.SelectDeckCard(cardData, index);
    }

    public void OnPointerExit(PointerEventData eventData) {

    }
}
