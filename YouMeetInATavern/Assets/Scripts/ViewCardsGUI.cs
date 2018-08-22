using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCardsGUI : MonoBehaviour {

    public Transform displayedCardPosition, offscreenCardPosition;
    public float cardOffset, cardMoveSpeed;

    private GameObject[] deckCards;
    private Transform[] cardPositions;

    private Transform deckCardsParent;

    private int currentCardIndex;

    void Start() {

    }

    void Update() {

    }

    public void Load(GameData data, List<string> unlockKeys) {
        deckCardsParent = new GameObject("ViewCardsGUI - DeckCards").transform;
        deckCardsParent.position = displayedCardPosition.position;

        // create all cards
        deckCards = new GameObject[unlockKeys.Count];
        for (int i = 0; i < unlockKeys.Count; i++) {
            GameObject card = CardFactory.CreateDeckCard(unlockKeys[i]);
            card.transform.parent = deckCardsParent;
            UnityUtility.MoveToLayer(card.transform, LayerMask.NameToLayer("Deck"));
            deckCards[i] = card;
        }

        // create a stack of positions
        cardPositions = new Transform[unlockKeys.Count];
        Vector3 currentPosition = displayedCardPosition.position;
        for (int i = 0; i < cardPositions.Length; i++) {
            cardPositions[i] = new GameObject("DeckCardTransform" + i).transform;
            cardPositions[i].parent = deckCardsParent;
            cardPositions[i].position = new Vector3(
                displayedCardPosition.position.x,
                displayedCardPosition.position.y,
                displayedCardPosition.position.z + i * cardOffset);
            cardPositions[i].rotation = displayedCardPosition.rotation;
        }

        // TESTING
        for (int i = 0; i < unlockKeys.Count; i++) {
            deckCards[i].transform.position = cardPositions[i].position;
            deckCards[i].transform.rotation = cardPositions[i].rotation;
        }
    }

    public void Display() {
        currentCardIndex = 0;
    }

    public void OnEnable() {
        InputController.deckCardSelectedEventHandler += RevealCard;
    }

    public void OnDisable() {
        InputController.deckCardSelectedEventHandler -= RevealCard;
    }

    private void RevealCard(CardData cardData, int index) {
        print("Now hovering over " + cardData.name + " at index " + index);
        if (index < currentCardIndex) {
            // move cards back onto stack
            for (int i = index; i < currentCardIndex; i++) {
                CardMoveController.Move(deckCards[i], offscreenCardPosition, cardPositions[i], cardMoveSpeed);
            }
        } else if (index > currentCardIndex) {
            // remove cards off stack
            for (int i = currentCardIndex; i < index; i++) {
                CardMoveController.Move(deckCards[i], cardPositions[i], offscreenCardPosition, cardMoveSpeed);
            }
        } else {
            // hover index is current index. stay put
        }

        currentCardIndex = index;

        // move all card positions forward so that revealed card is on top
        deckCardsParent.position = new Vector3(
            deckCardsParent.position.x,
            deckCardsParent.position.y,
            displayedCardPosition.position.z - index * cardOffset
            );
    }
}
