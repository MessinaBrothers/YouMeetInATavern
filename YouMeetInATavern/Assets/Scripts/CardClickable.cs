using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClickable : MonoBehaviour {

    public static event CardClickedEventHandler cardClickedEventHandler;
    public delegate void CardClickedEventHandler(GameObject card);

    public void Broadcast() {
        cardClickedEventHandler(gameObject);
    }
}
