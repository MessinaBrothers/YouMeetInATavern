using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour {

    public static event NPCEnteredTavernEventHandler npcEnteredTavernEventHandler;
    public delegate void NPCEnteredTavernEventHandler(GameObject card);

    public static event NPCRemovedEventHandler npcRemovedEventHandler;
    public delegate void NPCRemovedEventHandler(GameObject card);

    public static event NPCIntroduceEventHandler npcIntroStartEventHandler;
    public delegate void NPCIntroduceEventHandler(GameObject card);

    public static event NPCIntroducedEventHandler npcIntroEndEventHandler;
    public delegate void NPCIntroducedEventHandler(GameObject card);

    public static event NPCStartInTaverneventHandler npcStartInTaverneventHandler;
    public delegate void NPCStartInTaverneventHandler(GameObject card);

    public static event NPCRandomlyLeavesEventHandler npcRandomlyLeavesEventHandler;
    public delegate void NPCRandomlyLeavesEventHandler(GameObject card);

    public GameObject cardPrefab;

    public bool startIntro;

    private GameData data;

    private Transform cardParent;

    private List<GameObject> introducedNPCs;

    void Start() {
        data = FindObjectOfType<GameData>();

        introducedNPCs = new List<GameObject>();
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
        InputController.gameInitializedEventHandler += CreateCards;
        InputController.cardClickedEventHandler += HandleCardClick;
        InputController.startTavernEventHandler += ContinueDay;
        InputController.stopConverseEventHandler += IntroduceNextNPC;
        InputController.npcLeavesEventHandler += Goodbye;
        InputController.endResultsEventHandler += ReintroduceNPCs;
    }

    void OnDisable() {
        InputController.gameInitializedEventHandler -= CreateCards;
        InputController.cardClickedEventHandler -= HandleCardClick;
        InputController.startTavernEventHandler -= ContinueDay;
        InputController.stopConverseEventHandler -= IntroduceNextNPC;
        InputController.npcLeavesEventHandler -= Goodbye;
        InputController.endResultsEventHandler -= ReintroduceNPCs;
    }

    private void CreateCards() {
        // create parent for cards to go under
        GameObject go = new GameObject("NPCs");
        cardParent = go.transform;

        // create cards for each NPC
        foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
            GameObject card = CardFactory.CreateNPCCard(kvp.Key);
            card.transform.parent = cardParent;

            card.SetActive(false);
        }
    }

    private void Goodbye() {
        data.gameMode = GameData.GameMode.TAVERN;
        data.npcsInTavern.Remove(data.selectedCard);

        // remove a remaining NPC
        if (data.npcsInTavern.Count > 0) {
            GameObject rngCard = data.npcsInTavern[UnityEngine.Random.Range(0, data.npcsInTavern.Count)];
            data.npcsInTavern.Remove(rngCard);
            npcRandomlyLeavesEventHandler.Invoke(rngCard);
        }
    }

    public void RemoveNPCFromTavern(GameObject card) {
        //card.SetActive(false);
        npcRemovedEventHandler.Invoke(card);
    }

    private void HandleCardClick(GameObject card) {
        if (card.GetComponent<NPC>() == null) return;
        switch (data.gameMode) {
            case GameData.GameMode.INTRODUCE:
                if (card.GetComponent<NPC>().isBeingIntroduced == true) {
                    card.GetComponent<CardSFX>().PlayIntro();
                    npcIntroEndEventHandler.Invoke(card);
                    data.gameMode = GameData.GameMode.CONVERSE;
                    data.selectedCard = card;
                }
                break;
            case GameData.GameMode.CONVERSE:
                break;
            case GameData.GameMode.TAVERN:
                data.gameMode = GameData.GameMode.CONVERSE;
                data.selectedCard = card;
                break;
        }
    }

    private void IntroduceNextNPC() {
        // check for NPCs to reintroduce, then new NPCs
        if (data.npcsToReintroduce.Count > 0) {
            IntroduceNPC(data.npcsToReintroduce.Dequeue());
        } else if (data.npcsToIntroduce.Count > 0) {
            IntroduceNPC(data.npcsToIntroduce.Dequeue());
        } else {
            data.gameMode = GameData.GameMode.TAVERN;
        }
    }

    private void IntroduceNPC(GameObject card) {
        data.gameMode = GameData.GameMode.INTRODUCE;

        card.SetActive(true);

        introducedNPCs.Add(card);

        // move it anywhere offscreen so it doesn't appear at the beginning
        card.transform.position = new Vector3(0, 10, 0);

        NPC npc = card.GetComponent<NPC>();
        npc.isBeingIntroduced = true;

        // set the next dialogue. If the NPC has dialogue specific to the last scenario result, use it
        if (data.npc_dialogues[npc.key].ContainsKey(data.nextDialogueIntroKey)) {
            npc.nextDialogueID = data.nextDialogueIntroKey;
        } else {
            // otherwise, use the default into dialogue
            npc.nextDialogueID = GameData.DIALOGUE_INTRO;
        }

        // add NPC to tavern list
        data.npcsInTavern.Add(card);

        npcEnteredTavernEventHandler.Invoke(card);
        npcIntroStartEventHandler.Invoke(card);
    }

    private void IntroduceNPC(string npcKey) {
        // find the corresponding card GameObject
        foreach (Transform child in cardParent) {
            if (npcKey == child.gameObject.GetComponent<NPC>().key) {
                IntroduceNPC(child.gameObject);
                break;
            }
        }
    }

    private void ContinueDay() {
        startIntro = true;

        // place already-introduced NPCs in the tavern
        ActivateNPCs();

        // load list of NPCs to introduce
        if (data.scenario.day_introductions.ContainsKey(data.dayCount)) {
            foreach (string npcKey in data.scenario.day_introductions[data.dayCount]) {
                data.npcsToIntroduce.Enqueue(npcKey);
            }
        }

        // introduce the first NPC, if any
        IntroduceNextNPC();
    }

    private void ActivateNPCs() {
        foreach (GameObject card in introducedNPCs) {
            // activate the GameObject
            card.SetActive(true);
            // add NPC to npc list
            data.npcsInTavern.Add(card);
            // broadcast that the NPC has entered the tavern
            npcStartInTaverneventHandler.Invoke(card);

            npcEnteredTavernEventHandler.Invoke(card);
        }
    }

    private void ReintroduceNPCs() {
        /// TODO
        /// Reintroduce NPCs that have relevant dialogue
        /// e.g. Bartender after bringing her along on a quest
        /// Quest rewards i.e. Adventurer are added elsewhere
        /// All other already-introduced NPCs should just appear in the bar
        /// 

        List<GameObject> previousNPCs = new List<GameObject>(introducedNPCs);

        introducedNPCs.Clear();

        foreach (GameObject card in previousNPCs) {
            NPC npc = card.GetComponent<NPC>();
            // if NPC has a special dialogue
            if (data.npc_dialogues[npc.key].ContainsKey(data.nextDialogueIntroKey)) {
                // set their next dialogue
                npc.nextDialogueID = data.nextDialogueIntroKey;
                // reintroduce them
                data.npcsToReintroduce.Enqueue(card);
            } else {
                // if no specific dialogue, restart in tavern as an already-introdced NPC
                introducedNPCs.Add(card);
            }
        }
        
        //foreach (KeyValuePair<string, NPCData> kvp in data.npcData) {
        //    string npcKey = kvp.Key;
        //    NPCData npc = kvp.Value;
        //    // set the next dialogue. If the NPC has dialogue specific to the last scenario result, use it
        //    if (data.npc_dialogues[npcKey].ContainsKey(data.nextDialogueIntroKey)) {
        //        npc.nextDialogueID = data.nextDialogueIntroKey;
        //    } else {
        //        // otherwise, use the default into dialogue
        //        npc.nextDialogueID = GameData.DIALOGUE_INTRO;
        //    }
        //}
    }
}
