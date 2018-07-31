using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour {

    public GameObject cardPrefab;

    public bool addCard;

    public float cardDistance;
    public float cardRotationMax;

    public int cardCountMax;
    private int cardCount;

    void Start() {
        cardCount = 0;
    }

    void Update() {
        if (addCard) {
            addCard = false;
            AddCard();
        }
    }

    private void AddCard() {
        if (cardCount < cardCountMax) {
            GameObject card = Instantiate(cardPrefab);
            card.transform.parent = gameObject.transform;
            card.transform.position = new Vector3(
                transform.position.x,
                transform.position.y + cardCount * cardDistance,
                transform.position.z
                );
            //card.transform.rotation = Quaternion.Euler(
            //    transform.transform.rotation.x,
            //    transform.transform.rotation.y + Random.Range(-cardRotationMax, cardRotationMax),
            //    transform.transform.rotation.z
            //    );
            card.transform.Rotate(Vector3.forward, Random.Range(-cardRotationMax, cardRotationMax));
            cardCount += 1;
        }
    }
}
