using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public static event NPCIntroducedEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public static event NPCIntroduceEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroduceEventHandler(GameObject card);

    public GameObject cardPrefab;

    public bool startIntro;

    private GameData data;

    private GameObject card;

    void Start() {
        data = FindObjectOfType<GameData>();

        card = Instantiate(cardPrefab);
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (startIntro == true) {
                startIntro = false;
                
                npcIntroStartEventHandler.Invoke(card);
            }
        }
    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleCardClick;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.INTRODUCE && card == this.card) {
            card.GetComponent<CardSFX>().PlayIntro();
            npcIntroEndEventHandler.Invoke(card);
        }
    }
}
