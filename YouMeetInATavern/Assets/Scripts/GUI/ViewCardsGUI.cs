using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCardsGUI : MonoBehaviour {

    public Transform displayedCardPosition, offscreenCardPosition;
    public float cardOffset, cardMoveSpeed;
    public AudioClip[] cardFlipClips;

    private GameData data;
    private AudioSource audioSource;

    private GameObject[] deckCards;
    private string[] cardKeys;
    private Transform[] cardPositions;
    // what position index the card SHOULD be at
    // to account for gaps between cards (when not all cards are unlocked)
    public int[] cardPositionIndexes;

    private Transform deckCardsParent;

    private int currentCardIndex;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

    }

    public void Load(GameData data, List<string> unlockKeys) {
        this.data = data;
        cardKeys = new string[unlockKeys.Count];
        for (int i = 0; i < cardKeys.Length; i++) {
            cardKeys[i] = unlockKeys[i];
        }
        cardPositionIndexes = new int[unlockKeys.Count];
        
        deckCardsParent = new GameObject("ViewCardsGUI - DeckCards").transform;
        deckCardsParent.position = displayedCardPosition.position;

        // create all cards
        deckCards = new GameObject[unlockKeys.Count];
        for (int i = 0; i < unlockKeys.Count; i++) {
            GameObject card = CardFactory.CreateDeckCard(unlockKeys[i]);
            card.transform.parent = deckCardsParent;
            UnityUtility.MoveToLayer(card.transform, LayerMask.NameToLayer("Deck"));
            deckCards[i] = card;
            card.SetActive(false);
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

        // only show a deck of unlocked cards
        int nextPositionIndex = 0;

        for (int i = 0; i < cardKeys.Length; i++) {
            if (DeckController.Contains(cardKeys[i])) {
                cardPositionIndexes[i] = nextPositionIndex;
                deckCards[i].transform.position = cardPositions[nextPositionIndex].position;
                deckCards[i].transform.rotation = cardPositions[nextPositionIndex].rotation;
                CardMoveController.Move(deckCards[i], cardPositions[nextPositionIndex], cardPositions[nextPositionIndex], 0);
                deckCards[i].SetActive(true);
                nextPositionIndex += 1;
            } else {
                deckCards[i].SetActive(false);
            }
        }

        // reset all card positions
        deckCardsParent.position = new Vector3(
            deckCardsParent.position.x,
            deckCardsParent.position.y,
            displayedCardPosition.position.z
            );
    }

    public void Close() {
        for (int i = 0; i < cardKeys.Length; i++) {
            deckCards[i].SetActive(false);
        }
    }

    public void OnEnable() {
        InputController.deckCardSelectedEventHandler += RevealCard;
    }

    public void OnDisable() {
        InputController.deckCardSelectedEventHandler -= RevealCard;
    }

    private void RevealCard(CardData cardData, int index) {
        // play a sound
        audioSource.PlayOneShot(cardFlipClips[Random.Range(0, cardFlipClips.Length)]);

        //print("Now hovering over " + cardData.name + " at index " + index);
        if (index < currentCardIndex) {
            // move cards back onto stack
            for (int i = index; i < currentCardIndex; i++) {
                CardMoveController.Move(deckCards[i], offscreenCardPosition, cardPositions[cardPositionIndexes[i]], cardMoveSpeed);
            }
        } else if (index > currentCardIndex) {
            // remove cards off stack
            for (int i = currentCardIndex; i < index; i++) {
                CardMoveController.Move(deckCards[i], cardPositions[cardPositionIndexes[i]], offscreenCardPosition, cardMoveSpeed);
            }
        } else {
            // hover index is current index. stay put
        }

        currentCardIndex = index;

        // move all card positions forward so that revealed card is on top
        deckCardsParent.position = new Vector3(
            deckCardsParent.position.x,
            deckCardsParent.position.y,
            displayedCardPosition.position.z - cardPositionIndexes[index] * cardOffset
            );
    }
}
