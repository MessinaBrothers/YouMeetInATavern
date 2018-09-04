using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    public static event NPCEnteredTavernEventHandler npcEnteredTavernEventHandler;
    public delegate void NPCEnteredTavernEventHandler(GameObject card);

    public static event NPCIntroduceEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroduceEventHandler(GameObject card);

    public static event NPCIntroducedEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public static event NPCStartInTaverneventHandler npcStartInTaverneventHandler;
    public delegate void NPCStartInTaverneventHandler(GameObject card);

    public static event NPCRandomlyLeavesEventHandler npcRandomlyLeavesEventHandler;
    public delegate void NPCRandomlyLeavesEventHandler(GameObject card);

    public GameObject cardPrefab;

    private GameData data;

    private List<GameObject> introducedNPCs;

    void Start() {
        data = FindObjectOfType<GameData>();

        introducedNPCs = new List<GameObject>();
    }

    void Update() {
        if (data.gameMode == GameData.GameMode.INTRODUCE) {

        }
    }

    void OnEnable() {
        InputController.gameInitializedEventHandler += CreateCards;
        InputController.startTavernEventHandler += ContinueDay;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.stopConverseEventHandler += IntroduceNextNPC;
        InputController.npcLeavesEventHandler += Goodbye;
    }

    void OnDisable() {
        InputController.gameInitializedEventHandler -= CreateCards;
        InputController.startTavernEventHandler -= ContinueDay;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.stopConverseEventHandler -= IntroduceNextNPC;
        InputController.npcLeavesEventHandler -= Goodbye;
    }

    private void CreateCards() {
        // create parent for cards to go under
        Transform cardParent = new GameObject("NPCs").transform;

        // create cards for each NPC
        foreach (KeyValuePair<string, CardData> kvp in data.cardData) {
            if (kvp.Key.StartsWith("NPC_")) {
                GameObject card = CardFactory.CreateCard(kvp.Key);
                card.transform.parent = cardParent;

                data.npcs.Add(card);

                card.SetActive(false);  
            }
        }
    }

    private void Goodbye() {
        InputController.ChangeMode(GameData.GameMode.TAVERN);

        // remove a remaining NPC
        if (data.npcsInTavern.Count > 0) {
            GameObject rngCard = data.npcsInTavern[UnityEngine.Random.Range(0, data.npcsInTavern.Count)];
            npcRandomlyLeavesEventHandler.Invoke(rngCard);
        }
    }

    private void HandleCardClick(GameObject card) {
        if (card.GetComponent<NPC>() == null) return;
        switch (data.gameMode) {
            case GameData.GameMode.INTRODUCE:
                if (card.GetComponent<NPC>().isBeingIntroduced == true) {
                    card.GetComponent<CardSFX>().PlayIntro();
                    npcIntroEndEventHandler.Invoke(card);
                    InputController.ChangeMode(GameData.GameMode.CONVERSE);
                    data.selectedCard = card;
                }
                break;
            case GameData.GameMode.CONVERSE:
                break;
            case GameData.GameMode.TAVERN:
                InputController.ChangeMode(GameData.GameMode.CONVERSE);
                data.selectedCard = card;
                break;
        }
    }

    private void IntroduceNextNPC(GameObject lastCard) {
        // check for NPCs to reintroduce, then new NPCs
        //print("Checking NPC introductions...");
        if (data.npcsToReintroduce.Count > 0) {
            GameObject card = data.npcsToReintroduce.Dequeue();
            IntroduceNPC(card);
            //print("Reintroducing " + card);
        } else if (data.npcsToIntroduce.Count > 0) {
            string key = data.npcsToIntroduce.Dequeue();
            IntroduceNPC(key);
            //print("Introducing " + key);
        } else {
            InputController.ChangeMode(GameData.GameMode.TAVERN);
        }
    }

    private void IntroduceNPC(GameObject card) {
        InputController.ChangeMode(GameData.GameMode.INTRODUCE);

        card.SetActive(true);

        introducedNPCs.Add(card);

        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, 10, 0);

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;
        npc.isUnintroduced = false;

        // set the next dialogue
        if (data.npc_dialogues[npc.key].ContainsKey(data.nextDialogueIntroKey)) {
            // if the NPC has dialogue specific to LAST result, use it
            npc.nextDialogueID = data.nextDialogueIntroKey;
        } else {
            // otherwise, use the default intro dialogue
            npc.nextDialogueID = GameData.DIALOGUE_INTRO;
        }

        // add NPC to tavern list
        data.npcsInTavern.Add(card);

        npcEnteredTavernEventHandler.Invoke(card);
        npcIntroStartEventHandler.Invoke(card);
    }

    private void IntroduceNPC(string npcKey) {
        // find the corresponding card GameObject
        foreach (GameObject go in data.npcs) {
            NPC npc = go.GetComponent<NPC>();
            if (npcKey == npc.key) {
                IntroduceNPC(go);
                break;
            }
        }
    }

    private void ContinueDay() {
        // initialize each npc key in this scenario
        foreach (string key in data.scenario.npcs) {
            foreach (GameObject go in data.npcs) {
                NPC npc = go.GetComponent<NPC>();
                if (npc.key == key) {
                    Initialize(npc);
                }
            }
        }

        // introduce the first NPC, if any
        IntroduceNextNPC(null);
    }

    private void Initialize(NPC npc) {
        // if the NPC has NOT been introduced
        if (npc.isUnintroduced == true) {
            // introduce the NPC
            data.npcsToIntroduce.Enqueue(npc.key);
            //print("Going to introduce " + npc.key);
        // if the NPC has something to say about the last scenario result
        } else if (data.npc_dialogues[npc.key].ContainsKey(data.nextDialogueIntroKey)) {
            // set their next dialogue
            npc.nextDialogueID = data.nextDialogueIntroKey;
            // reintroduce them
            data.npcsToReintroduce.Enqueue(npc.gameObject);
            //print("Going to reintroduce " + npc.key);
        } else {
            // activate the NPC in the tavern
            ActivateNPC(npc.gameObject);
            //print("Going to activate " + npc.key);
        }
    }

    private void ActivateNPC(GameObject card) {
        // activate the GameObject
        card.SetActive(true);
        // add NPC to npc list
        data.npcsInTavern.Add(card);
        // broadcast that the NPC has entered the tavern
        npcStartInTaverneventHandler.Invoke(card);

        npcEnteredTavernEventHandler.Invoke(card);
    }
}
