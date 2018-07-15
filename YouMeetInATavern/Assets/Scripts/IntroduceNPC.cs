using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroduceNPC : MonoBehaviour {

    public static event NPCIntroduceEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroduceEventHandler(GameObject card);

    public static event NPCIntroducedEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public GameObject cardPrefab;

    public bool startIntro;

    private GameData data;

    private GameObject card;

    void Start() {
        data = FindObjectOfType<GameData>();
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (startIntro == true) {
                startIntro = false;

                //npcIntroStartEventHandler.Invoke(card);
            }
        }
    }

    void OnEnable() {
        CardClickable.cardClickedEventHandler += HandleCardClick;
        InputController.startTavernEventHandler += StartDay;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        InputController.startTavernEventHandler -= StartDay;
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.INTRODUCE && card == this.card) {
            card.GetComponent<CardSFX>().PlayIntro();
            npcIntroEndEventHandler.Invoke(card);
        }
    }

    private void StartDay() {
        startIntro = true;

        card = Instantiate(cardPrefab);
        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, 1000, 0);
        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;
        // set the next dialogue to be intro dialogue
        npc.nextDialogueID = GameData.DIALOGUE_INTRO;

        npcIntroStartEventHandler.Invoke(card);
    }
}
