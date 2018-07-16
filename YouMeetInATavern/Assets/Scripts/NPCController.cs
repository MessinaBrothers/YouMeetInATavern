using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour {

    public static event NPCCreatedEventHandler npcCreatedEventHandler;
    public delegate void NPCCreatedEventHandler(GameObject card);

    public static event NPCRemovedEventHandler npcRemovedEventHandler;
    public delegate void NPCRemovedEventHandler(GameObject card);

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
        InputController.startTavernEventHandler += ContinueDay;
    }

    void OnDisable() {
        CardClickable.cardClickedEventHandler -= HandleCardClick;
        InputController.startTavernEventHandler -= ContinueDay;
    }

    public void RemoveNPCFromTavern(GameObject card) {
        card.SetActive(false);
        data.npcs.Remove(card);
        npcRemovedEventHandler.Invoke(card);
    }

    private void HandleCardClick(GameObject card) {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {
            if (card.GetComponent<NPC>().isBeingIntroduced == true) {
                card.GetComponent<CardSFX>().PlayIntro();
                npcIntroEndEventHandler.Invoke(card);
            }
        }
    }

    private void ContinueDay() {
        startIntro = true;

        card = Instantiate(cardPrefab);
        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, 1000, 0);
        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;
        // set the next dialogue to be intro dialogue
        npc.nextDialogueID = GameData.DIALOGUE_INTRO;

        // add NPC to npc list
        data.npcs.Add(card);

        npcCreatedEventHandler.Invoke(card);

        npcIntroStartEventHandler.Invoke(card);
    }
}
